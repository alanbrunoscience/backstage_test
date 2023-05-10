
namespace Jazz.Covenant.Application.Data.CovenantQuerysTemplates
{
    public static class CovenantQuery
    {
        public const string SELECT_COVENANT_ENDORSER = @"SELECT e.Id as EndoserId, e.EndoserIdentifier , c.IdentifierInEndoser
                                                           FROM [dbo].[Covenants] c
                                                          INNER JOIN Endoser as e ON e.Id = c.EndoserId /**where**/";

        public const string SELECT_COVENANT_MODALITY = @"SELECT Covenants.Id,Covenants.Name,Covenants.Organization,Covenants.Active, m.Id as 'ModalityId',m.Name as 'ModalityName'
                                                        FROM [dbo].[Covenants] 
                                                        Inner Join ModalityCovenant AS mC ON mC.CovenantId=Covenants.Id
                                                        Inner Join Modality as m on m.Id=mC.ModalityId
													 /**where**/";

        public const string SELECT_COVENANTS_MODALITYS = @$"{SELECT_COVENANT_MODALITY} /**orderby**/
                                                            OFFSET @skip ROWS
                                                            FETCH NEXT @size ROWS ONLY";

        public const string COUNT_SELECT_COVENANT_MODALITY = @"SELECT  COUNT(DISTINCT	Covenants.Id)
                                                        FROM [dbo].[Covenants] 
                                                        Inner Join ModalityCovenant AS mC ON mC.CovenantId=Covenants.Id
                                                        Inner Join Modality as m on m.Id=mC.ModalityId /**where**/";

        public const string COUNT_SELECT_MODALITY = @"SELECT Count(IdentificationModality) FROM [dbo].[Modality] /**where**/";

        public const string COUNT_SELECT_MODALITY_ID = @"SELECT Count(Id) FROM [dbo].[Modality] /**where**/";
        public const string COUNT_SELECT_ENDOSER_ID = @"SELECT COUNT(*) FROM [dbo].Endoser /**where**/ ";
        public const string SELECT_CONVENANT_FAVORITE = @"SELECT Id FROM [dbo].CovenantFavorite /**where**/";

        public const string SELECT_HISTORY_ENDOSERMANT = @"SELECT StatusEndosament FROM [dbo].[MarginEndosamentStatusHistory] /**where**/ ";
    }

    public static class MarginReserveStatusQuery
    {
        public const string SELECT_MARGIN_RESERVE_SATTUS = @"SELECT mr.MarginReserveStatus as Status FROM MarginReserve mr /**where**/";
    }

    public static class MarginReserveContractNumber
    {
        public const string SELECT_MARGIN_CONTRACT_NUMBER = @"SELECT top 1 mr.ContractNumber as Status FROM MarginReserve mr /**where**/ order by mr.InsertDate desc ";
    }
}