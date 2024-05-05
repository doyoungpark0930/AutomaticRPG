using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

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
    [SerializeField]
    private Weapon equippedWeapon;
    public Weapon EquippedWeapon
    {
        get { return equippedWeapon; }
        set //Job에 해당하는 무기만 장착 가능
        {
            if (value == null || IsWeaponValidForJob(value.Type, job))
            {
                equippedWeapon = value;
            }
            else
            {
                throw new InvalidOperationException($"{value.Name} cannot be equipped by a {job}.");
            }
        }
    }
    public Armor EquippedArmor;
    [SerializeField]
    private Element element;
    public Element Element { get { return element; } }
    public Skill Skill;


    public int BaseAttackPower;

    // 총 공격력은 기본 공격력 + 무기 데미지
    public int TotalAttackPower
    {
        get 
        { 
            return BaseAttackPower + ((equippedWeapon != null && equippedWeapon.Name != "") ? equippedWeapon.Damage : 0) 
                + ((EquippedArmor != null && EquippedArmor.Name != "") ? EquippedArmor.Damage : 0); 
        }
    }

    public int Health;
    public int Defense;
    public float AttackSpeed = 1.0f;
    public float CombatPower
    {
        get
        {
            return (TotalAttackPower * 2 + Defense * 1 + Health * 0.1f) * AttackSpeed;
        }
    }
    public Character(string name, string level, string grade, string job, string element, string skill,
                     string attackPower, string health, string defense)
    {
        this.name = name;
        this.Level = int.Parse(level);
        this.Grade = int.Parse(grade);
        this.job = (JobType)Enum.Parse(typeof(JobType), job);
        this.element = (Element)Enum.Parse(typeof(Element), element);
        this.Skill = new Skill(skill);
        this.BaseAttackPower = int.Parse(attackPower);
        this.Health = int.Parse(health);
        this.Defense = int.Parse(defense);
    }

   

    private bool IsWeaponValidForJob(WeaponType weaponType, JobType jobType)
    {
        switch (jobType)
        {
            case JobType.Warrior:
                return weaponType == WeaponType.Sword;
            case JobType.Archer:
                return weaponType == WeaponType.Bow;
            case JobType.Mage:
                return weaponType == WeaponType.Staff;
            default:
                return false;
        }
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
    public int Defense = 0;
    public int Health = 0;
    public int Speed = 0;

    public string EquippedCharacterName = "";

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
    public int Damage = 0;
    public int Health = 0;

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
