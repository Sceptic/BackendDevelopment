namespace Domain.Helpers
{
    public static class Helpers
    {
        //Wordt binnen de invariants gebruikt om ervoor te zorgen dat je niet bij elke foutmelding een nieuwe exception moet toevoegen.
        public static void Require(bool condition, string message)
        {
            if (!condition) throw new ArgumentException(message);
        }
    }
}
