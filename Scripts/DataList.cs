using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

public class AllDatabase   //json직렬화를 위해 클래스 따로 형성
{
    public List<Character> allCharacter;
    public List<Weapon> allWeapon;
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



// Character class definition
[System.Serializable]
public class Character
{
    public string Name { get; }
    public int Level;
    public int Grade;
    public JobType Job { get; }
    public Weapon EquippedWeapon;
    public Armor EquippedArmor;
    public Element Element { get; }
    public Skill Skill;

    public Character(string name, string level, string grade, string job, string element, string skill)
    {
        Name = name;
        Level = int.Parse(level);
        Grade = int.Parse(grade);
        Job = (JobType)Enum.Parse(typeof(JobType), job);
        Element = (Element)Enum.Parse(typeof(Element), element);
        Skill = new Skill(skill);
    }
}


// Weapon class definition
[System.Serializable]
public class Weapon
{
    public string Name;
    public WeaponType Type { get; }
    public int Damage;

    public Weapon(string name, string weapontype, string damage)
    {
        Name = name;
        Type = (WeaponType)Enum.Parse(typeof(WeaponType), weapontype);
        Damage = int.Parse(damage);
    }
}

// Armor class definition
[System.Serializable]
public class Armor
{
    public string Name;
    public int Defense;

    public Armor(string name, int defense)
    {
        Name = name;
        Defense = defense;
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
        Name = name;
    }
}

// JobUtils static class
public static class JobUtils
{
    private static readonly Dictionary<JobType, WeaponType> allowedWeapons = new Dictionary<JobType, WeaponType>
    {
        { JobType.Warrior, WeaponType.Sword },
        { JobType.Mage, WeaponType.Staff },
        { JobType.Archer, WeaponType.Bow }
    };

    public static WeaponType GetAllowedWeaponType(JobType job)
    {
        if (allowedWeapons.TryGetValue(job, out WeaponType allowedWeapon))
        {
            return allowedWeapon;
        }

        // When there is no matched weapon found, instead of throwing an exception, we return a default value.
        return default;
    }
}