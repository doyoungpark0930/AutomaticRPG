using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;



public class DataManager : MonoBehaviour
{
    public static DataManager instance = null;

    [SerializeField] TextAsset CharacterDB;
    [SerializeField] TextAsset WeaponDB;
    public List<Character> allCharacterList; //캐릭터 기본 DB
    public static List<Character> MyCharacterList; //내 캐릭터 DB
    public List<Weapon> allWeaponList; //무기 기본 DB
    public static List<Weapon> MyWeaponList; //내 무기 DB


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        Initialize();
    }

    void Initialize()
    {
        string[] line_0 = CharacterDB.text.Substring(0, CharacterDB.text.Length - 1).Split('\n'); //CharacterDB(기본 캐릭터 정보) 줄 단위로 받아서 넣음
        string[] line_1 = WeaponDB.text.Substring(0, WeaponDB.text.Length - 1).Split('\n'); // WeaponDB(기본 무기 정보) 줄 단위로 받아서 넣음
        for (int i = 0; i < line_0.Length; i++)
        {
            string[] row = line_0[i].Split('\t'); //CharacterDB 텝 단위로 받아서 넣음
            allCharacterList.Add(new Character(row[0], row[1], row[2], row[3], row[4], row[5]));
        }
        for (int i = 0; i < line_1.Length; i++)
        {
            string[] row = line_1[i].Split('\t'); //WeaponDB 텝 단위로 받아서 넣음
            allWeaponList.Add(new Weapon(row[0], row[1], row[2]));
        }
        Save();
        Load();
    }

   
    void Save()
    {
        AllDatabase allDatabase = new AllDatabase();
        allDatabase.allCharacter = allCharacterList;
        allDatabase.allWeapon = allWeaponList;

        string jdata = JsonUtility.ToJson(allDatabase); //직렬화
        File.WriteAllText(Application.dataPath + "/Resources/MyAllDatabase.txt", jdata);

    }

    void Load()
    {

        string jdata = File.ReadAllText(Application.dataPath + "/Resources/MyAllDatabase.txt");
        AllDatabase allDatabase = JsonUtility.FromJson<AllDatabase>(jdata); //역직렬화
        MyCharacterList = allDatabase.allCharacter;
        MyWeaponList = allDatabase.allWeapon;

    }
}







