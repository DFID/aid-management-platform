using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;

namespace AMP.Utilities
{
    public class DemoIdentityManager : IIdentityManager
    {
        public Person GetPersonByUserName(string userName)
        {

            Person person;

            switch (userName)
            {
                case "A-Inputter":
                    person = new Person
                    {
                        EmpNo = "111111",
                        Department = "Government Aid",
                        DisplayName = "An Inputter",
                        EmailAddress = "A-Inputter@AMPDemo.com",
                        UserName = "A-Inputter"
                    };
                    return person;
                case "A-SRO":
                    person = new Person
                    {
                        EmpNo = "222222",
                        Department = "Government Aid",
                        DisplayName = "An SRO",
                        EmailAddress = "A-SRO@AMPDemo.com",
                        UserName = "A-SRO"
                    };
                    return person;
                case "A-Adviser":
                    person = new Person
                    {
                        EmpNo = "333333",
                        Department = "Government Aid",
                        DisplayName = "An Adviser",
                        EmailAddress = "A-Adviser@AMPDemo.com",
                        UserName = "A-Adviser"
                    };
                    return person;
                case "A-TeamMember":
                    person = new Person
                    {
                        EmpNo = "444444",
                        Department = "Government Aid",
                        DisplayName = "A TeamMember",
                        EmailAddress = "A-TeamMember@AMPDemo.com",
                        UserName = "A-TeamMember"
                    };
                    return person;
                case "A-OfficeHead":
                    person = new Person
                    {
                        EmpNo = "555555",
                        Department = "Government Aid",
                        DisplayName = "A OfficeHead",
                        EmailAddress = "A-OfficeHead@AMPDemo.com",
                        UserName = "A-OfficeHead"
                    };
                    return person;
                default:
                    return null;
            }

        }

    }
}