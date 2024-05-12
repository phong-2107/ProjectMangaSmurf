namespace ProjectMangaSmurf.Models.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }  // Unique identifier for the role, if applicable
        public string Name { get; set; }  // Display name of the role
        public string Description { get; set; }  // A brief description of what the role entails

        // You can add additional fields that might be relevant to your role management:
        public bool IsActive { get; set; }  // Indicates if the role is active or not
    }
}
