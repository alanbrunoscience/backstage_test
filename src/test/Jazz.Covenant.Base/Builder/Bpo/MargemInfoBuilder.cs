using Bpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Covenant.Base.Builder.Bpo
{
    public class MargemInfoBuilder
    {
        private MargemInfo margemInfo;

        public MargemInfoBuilder()
        {
            margemInfo = new MargemInfo()
            {
                autorizada = true,
                dataRepasse = DateTime.Now,
                dataValidadeAutorizacao = DateTime.Now,
                indexReservaMargem = "teste",
                limiteMaximoContratos = true,
                observacao = "ddd",
                tipoProduto = 1,
                valorDisponivel = 40,
                valorDisponivel70 = 50,
                valorParcelaPortada = 20,
                valorReal = 100,
                valorReserva = 200
            };
        }

        public MargemInfoBuilder WithAutorizada(bool autorizada)
        {
            margemInfo.autorizada = autorizada;
            return this;
        }
        public MargemInfoBuilder WithDataRepasse(DateTime dataRepasse)
        {
            margemInfo.dataRepasse = dataRepasse;
            return this;
        }
        public MargemInfoBuilder WithDataValidadeAutorizacao(DateTime dataValidadeAutorizacao)
        {
            margemInfo.dataValidadeAutorizacao = dataValidadeAutorizacao;
            return this;
        }
        public MargemInfoBuilder WithIndexReservaMargem(string indexReservaMargem)
        {
            margemInfo.indexReservaMargem = indexReservaMargem;
            return this;
        }
        public MargemInfoBuilder WithLimiteMaximoCotratos(bool limiteMaximoContratos)
        {
            margemInfo.limiteMaximoContratos = limiteMaximoContratos;
            return this;
        }

        public MargemInfoBuilder WithObservacao(string observacao)
        {
            margemInfo.observacao = observacao;
            return this;
        }
        public MargemInfoBuilder WithTipoProduto(int tipoProduto)
        {
            margemInfo.tipoProduto = tipoProduto;
            return this;
        }
        public MargemInfoBuilder WithValorDisponivel(double valorDisponivel)
        {
            margemInfo.valorDisponivel = valorDisponivel;
            return this;
        }
        public MargemInfoBuilder WithValorDisponivel70(double valorDisponivel70)
        {
            margemInfo.valorDisponivel70 = valorDisponivel70;
            return this;
        }
        public MargemInfoBuilder WithValorParcelaPortada(double valorParcelaPortada)
        {
            margemInfo.valorParcelaPortada = valorParcelaPortada;
            return this;
        }
        public MargemInfoBuilder WithValorReal(double valorReal)
        {
            margemInfo.valorReal = valorReal;
            return this;
        }
        public MargemInfoBuilder WithValorReserval(double valorReserva)
        {
            margemInfo.valorReserva = valorReserva;
            return this;
        }
        public MargemInfo Build()
        {
            return margemInfo;
        }
    }
}
