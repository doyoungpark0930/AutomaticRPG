using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using JetBrains.Annotations;

public class AllDatabase   //json직렬화를 위해 클래스 따로 형성
{
    public MyInfo myInfo;
    public List<Character> allCharacter;
    public List<Weapon> allWeapon;
    public List<Armor> allArmor;
}

// Enumerations
public enum JobType
{
    Warrior, Mage, Archer
}

public enum Element
{
    Fire, Earth, Water, Wind
}

public enum WeaponType
{
    Sword, Bow, Staff
}


[System.Serializable]
public class MyInfo
{
    public string nickName = "Unknown";

    public string Progress = "Not started"; //진행도
    public int Exp = 0; //경험치
    public int Bread = 0;
    public int Gold = 0;

}


// Character class definition
[System.Serializable]
public class Character
{
    [SerializeField]  //private로 선언된 변수가 직렬화 되도록 하기 위함
    private string name;
    public string Name { get { return name; } }
    public int Level;
    public int grade;

    public int Grade
    {
        get { return grade; }
        set
        {
            if (value < 1 || value > 3)
            {
                throw new ArgumentException("Grade must be between 1 and 3.");
                // 혹은 유효하지 않은 값에 대해 기본값 설정
                // grade = 1; // 기본값으로 설정
            }
            else
            {
                grade = value;
            }
        }
    }
    [SerializeField]
    private JobType job;
    public JobType Job { get { return job; } }
    public Weapon EquippedWeapon;
    public Armor EquippedArmor;
    [SerializeField]
    private Element element;
    public Element Element { get { return element; } }
    public Skill Skill;

    public Character(string name, string level, string grade, string job, string element, string skill)
    {
        this.name = name;
        this.Level = int.Parse(level);
        this.Grade = int.Parse(grade);
        this.job = (JobType)Enum.Parse(typeof(JobType), job);
        this.element = (Element)Enum.Parse(typeof(Element), element);
        this.Skill = new Skill(skill);

    }
}


// Weapon class definition
[System.Serializable]
public class Weapon
{
    public string Name;
    [SerializeField]
    private WeaponType type;
    public WeaponType Type { get { return type; } }
    public int Damage;

    public Weapon(string name, string weapontype, string damage)
    {
        this.Name = name;
        this.type = (WeaponType)Enum.Parse(typeof(WeaponType), weapontype);
        this.Damage = int.Parse(damage);
    }
}

// Armor class definition
[System.Serializable]
public class Armor
{
    public string Name;
    public int Defense;

    public Armor(string name, string defense)
    {
        this.Name = name;
        this.Defense = int.Parse(defense);
    }
}

// Skill class definition
[System.Serializable]
public class Skill
{
    public string Name;
    public string Description;
    public int Power;

    public Skill(string name)
    {
        this.Name = name;
    }
}
