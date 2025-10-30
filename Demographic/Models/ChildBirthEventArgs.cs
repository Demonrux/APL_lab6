namespace Demographic.Models
{
    public class ChildBirthEventArgs : EventArgs
    {
        public Gender ChildGender { get; }
        public int BirthYear { get; }

        public ChildBirthEventArgs(Gender childGender, int birthYear)
        {
            ChildGender = childGender;
            BirthYear = birthYear;
        }
    }

}
