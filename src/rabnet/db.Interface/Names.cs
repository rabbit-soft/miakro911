using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class RabName : IData
    {
        public readonly int ID;
        public string Name;
        /// <summary>
        /// Фамилия по матери
        /// </summary>
        public readonly string Surname;
        public readonly int Use;
        public readonly DateTime ReleaseDate;
        public readonly Rabbit.SexType Sex;

        public RabName(int id, String name, String surname, String sex, int use, DateTime dt)
        {
            this.ID = id;
            this.Name = name;
            this.Surname = surname;
            this.Sex = Rabbit.GetSexType(sex);
            //this.sex = sex;
            //if (sex == "male")
            //    this.Sex = "м";
            //else
            //    this.Sex = "ж";
            this.Use = use;
            this.ReleaseDate = dt;
        }
        public RabName(int id, String name, String surname, String sex):this(id, name, surname, sex,0,DateTime.MinValue)
        {
        }
    }

    public class RabNamesList : List<RabName>
    {
        public RabName Search(int nID)
        {
            foreach (RabName rn in this)
            {
                if (rn.ID == nID)
                    return rn;
            }
            return null;
        }

        public RabName Search(string name, Rabbit.SexType sex)
        {
            foreach (RabName rn in this)
            {
                if (rn.Name == name && rn.Sex == sex)
                    return rn;
            }
            return null;
        }
    }
}
