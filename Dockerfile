FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim AS build-env

WORKDIR /app

ARG PAT_FOR_NUGET
ARG GITHUB_REPO_OWNER

COPY src .

RUN dotnet nuget add source \
    --username "ArbotCross" \
    --password $PAT_FOR_NUGET \
    --store-password-in-clear-text \
    --name Jazz "https://nuget.pkg.github.com/$GITHUB_REPO_OWNER/index.json"

RUN dotnet restore && dotnet publish *.sln -c Release -o out

#####
FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim

ARG DATADOG_AGENT_URL
ARG NAME_STARTUP_FOLDER

ENV CORECLR_ENABLE_PROFILING="1"
ENV CORECLR_PROFILER="{846F5F1C-F9AE-4B07-969E-05C26BC060D8}"
ENV CORECLR_PROFILER_PATH="/opt/datadog/Datadog.Trace.ClrProfiler.Native.so"
ENV DD_INTEGRATIONS="/opt/datadog/integrations.json"
ENV DD_DOTNET_TRACER_HOME="/opt/datadog"
ENV DD_ENV="prod"
ENV DD_LOGS_INJECTION="true"
ENV NAME_STARTUP_FOLDER=$NAME_STARTUP_FOLDER

WORKDIR /app

RUN apt-get update && \
    apt-get install libcap2-bin -y && \
    setcap CAP_NET_BIND_SERVICE=+eip /usr/share/dotnet/dotnet

RUN useradd -u 8899 armstrong

COPY --chown=armstrong:armstrong --from=build-env /app/out .

RUN mkdir /home/armstrong \
    && chmod --recursive +w /home/armstrong \
    && chown armstrong:armstrong --recursive /home/armstrong \
    && apt-get update \
    && apt-get install -y curl tzdata libgdiplus wget \
    && wget -c $DATADOG_AGENT_URL -O datadog-dotnet-apm.deb \
    && dpkg -i datadog-dotnet-apm.deb \
    && rm -f datadog-dotnet-apm.deb

EXPOSE 80

USER armstrong

ENTRYPOINT dotnet $NAME_STARTUP_FOLDER