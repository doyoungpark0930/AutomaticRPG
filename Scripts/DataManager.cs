using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;


public class DataManager : MonoBehaviour
{
    public TextAsset CharacterDB;
    public List<Character> allCharacterList;


    void Start()
    {
        string[] line = CharacterDB.text.Substring(0, CharacterDB.text.Length - 1).Split('\n');
        for(int i = 0; i < line.Length; i++)
        {//AllItemList.Add(new Item(row[0], row[1], row[2], row[3], row[4] == "TRUE", row[5]));
            string[] row = line[i].Split('\t');
            allCharacterList.Add(new Character(row[0],row[1],row[2],row[3],row[4],row[5]));
        }
        print(allCharacterList[1].Job);
    }
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
    public JobType Job{get;}
    public Weapon EquippedWeapon;
    public Armor EquippedArmor;
    public Element Element { get; }
    public Skill Skill;

    public Character(string name, string level,string grade, string job, string element, string skill)
    {
        Name = name;
        Level = int.Parse(level);
        Grade = int.Parse(grade);
        Job = (JobType)Enum.Parse(typeof(JobType), job);
        Element = (Element)Enum.Parse(typeof(Element), element);
        Skill = new Skill(skill);
    }
}
/*
굳이 저렇게 하지말고, 그냥 데이터 집어넣으면 알아서 무기 찾아주도록. 저거는 그냥 보완사항일 뿐?
null들어가면 기본무기 장착으로 해주기.
일단 캐릭터 데이터3개와 무기 데이터 3개를 넣는다. 각각 다른 종류로. 기본 정보만
*/

// Weapon class definition
[System.Serializable]
public class Weapon
{
    public string Name;
    public WeaponType Type;
    public int Damage;

    public Weapon(string name, WeaponType type, int damage)
    {
        Name = name;
        Type = type;
        Damage = damage;
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


