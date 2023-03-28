namespace CerealAPI.DTO
{
    public class CerealPicturesDTO
    {
        public int Id { get; set; }
        public Byte[] Picture { get; set; }

        public CerealPicturesDTO(int id, Byte[] picture) 
        {
            Id = id;
            Picture = picture;
        }
    }
}
