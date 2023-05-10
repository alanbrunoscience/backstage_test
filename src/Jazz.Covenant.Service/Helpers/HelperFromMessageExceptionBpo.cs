
public static class HelperFromMessageExceptionBpo
{
    public static string ErroMessageConsultMargin(string erroMessage)
    {
        return erroMessage.Split(":")[2];
    }
}

