using System;
using System.Collegctions.Generic;                            //Spelling mistake in Collections
using System.Linq;

namespace Utility.Valocity.ProfileHelper
{
    public class People                                     //Class is violating SRP 
    {
        private static readonly DateTimeOffset Under16 = DateTimeOffset.UtcNow.AddYears(-15); //Use proper indentation and spacing
        public string Name { get; private set; }
     public DateTimeOffset DOB { get; private set; }
     public People(string name) : this(name, Under16.Date) { } //There is no date property on datetime
     public People(string name, DateTime dob)
        {                                                   //Data type for dob should match with property type DateTimeOffset
            Name = name;
            DOB = dob;
     }}                                                      // Extra closing brace

    public class BirthingUnit
    {
        /// <summary>
        /// MaxItemsToRetrieve
        /// </summary>
        private List<People> _people;

        public BirthingUnit()
        {
            _people = new List<People>();
        }

        /// <summary>
        /// GetPeoples //No description of function mentioned and method name is different from the summary, it should be GetPeople
        /// </summary>
        /// <param name="j"></param> //No description for the parameters and parameter name is different from the method signature, it should be i
        /// <returns>List<object></returns>
        public List<People> GetPeople(int i) //Use meaningful variable names, i is not a good name for a parameter
        {
            for (int j = 0; j < i; j++)
            {
                try
                {
                    // Creates a dandon Name            //Typo in comment, should be random instead of dandon
                    string name = string.Empty;
                    var random = new Random();
                    if (random.Next(0, 1) == 0)
                    {                                   //should not use random inside loop, it will generate same number in each iteration
                        name = "Bob";                   //Hardcoded names, name should come from constant file
                    }
                    else {
                        name = "Betty";                 // what is the business logic for deciding names? Its not clear from the code. Also, hardcoded names should be moved to a constant file
                    }
                    // Adds new people to the list
                    _people.Add(new People(name, DateTime.UtcNow.Subtract(new TimeSpan(random.Next(18, 85) * 356, 0, 0, 0)))); 
                }
                catch (Exception e)
                {
                    // Dont think this should ever happen
                    throw new Exception("Something failed in user creation"); //Exception is rethrown without inner exception, it will lose the original exception . Also move the message to constants file
                }
            }
            return _people;
        }

        private IEnumerable<People> GetBobs(bool olderThan30)
        {
            return olderThan30 ? _people.Where(x => x.Name == "Bob" && x.DOB >= DateTime.Now.Subtract(new TimeSpan(30 * 356, 0, 0, 0))) : _people.Where(x => x.Name == "Bob"); //To find people older than 30, we should use <= and it should be 365 for days in year
        }

        public string GetMarried(People p, string lastName)
        {
            if (lastName.Contains("test")) //Magic string used in comparison,move it to constant file
                return p.Name;
            if ((p.Name.Length + lastName).Length > 255)
            {
                (p.Name + " " + lastName).Substring(0, 255);  //This is calculated but not used.
            }

            return p.Name + " " + lastName;
        }
    }
}
