namespace LegacyMonolith.Models
{
    //De Dto's (Data transfer objects) worden gebruikt en gevuld om .jsons te serializen tijdens het versturen van data binnen de API. Deze worden gevuld met de DbModel klassen.
    //Het verschil tussen de Dto's en de DbModels is dat de Dto's geen relatiemapping hebben, zij zijn platte C# objecten zonder ORM logica. Dit is noodzakelijk want de relatiekaart binnen
    //de DbModels is niet mogelijk om te mappen bij een json-vertaling. C# leest DbModels namelijk letterlijk en probeerd de relaties te mappen, waarbij het in een cyclus valt en crasht,
    //zie uitleg daarover in de DbModels.

    public class AccountDto
    {
        public int AccountId { get; set; }
    }
}
