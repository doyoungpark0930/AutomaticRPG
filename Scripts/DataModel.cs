using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;


public class DataModel : MonoBehaviour
{
    public static DataModel instance = null;

    public Action OnSaveRequested; //Save를 직접적으로 접근하는 것을 방지하기 위한 Save이벤트

    public DataModel()
    {
        OnSaveRequested = Save; // Delegate에 Save 메서드 연결
    }

    [SerializeField] TextAsset CharacterDB;
    [SerializeField] TextAsset WeaponDB;
    [SerializeField] TextAsset ArmorDB;

    public MyInfo myInfo = new MyInfo();

    public List<Character> allCharacterList; //캐릭터 기본 DB
    public List<Character> MyCharacterList; //내 캐릭터 DB

    public List<Weapon> allWeaponList; //무기 기본 DB
    public List<Weapon> MyWeaponList; //내 무기 DB

    public List<Armor> allArmorList; //방어구 기본 DB
    public List<Armor> MyArmorList; //내 방어구 DB



    public Sprite[] CharacterSprite;
    public Sprite[] JobSprite;
    public Sprite[] WeaponSprite;
    public Sprite[] ArmorSprite;
    public Sprite[] ElementSprite;


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
        string[] line_2 = ArmorDB.text.Substring(0, ArmorDB.text.Length - 1).Split('\n'); // ArmorDB(기본 방어구 정보) 줄 단위로 받아서 넣음
        for (int i = 0; i < line_0.Length; i++) 
        {
            string[] row = line_0[i].Split('\t'); //CharacterDB 텝 단위로 받아서 넣음
            allCharacterList.Add(new Character(row[0], row[1], row[2], row[3], row[4], row[5], row[6] ,row[7], row[8]));
        }
        for (int i = 0; i < line_1.Length; i++)
        {
            string[] row = line_1[i].Split('\t'); //WeaponDB 텝 단위로 받아서 넣음
            allWeaponList.Add(new Weapon(row[0], row[1], row[2]));
        }
        for (int i = 0; i < line_2.Length; i++)
        {
            string[] row = line_2[i].Split('\t'); //ArmorDB 텝 단위로 받아서 넣음
            allArmorList.Add(new Armor(row[0], row[1]));

        }

        Load();
        //Save();
        ReconnectEquipmentReferences();
    }


    void Save()
    {
        AllDatabase allDatabase = new AllDatabase();

        /*
        MyCharacterList[0].EquippedWeapon = MyWeaponList[0];
        MyCharacterList[0].EquippedArmor = MyArmorList[0];
        MyCharacterList[2].EquippedWeapon = MyWeaponList[1];
        MyCharacterList[1].EquippedArmor = MyArmorList[1];
        var rand = new System.Random();

        

        for (int i = 0; i < MyCharacterList.Count; i++)
        {
            MyCharacterList[i].Level = rand.Next(1, 101);  // Random level between 1 and 100
        }


  
        allDatabase.myInfo = new MyInfo();
        allDatabase.myInfo.nickName = "도영";
        allDatabase.myInfo.Progress = "1-1";
        allDatabase.myInfo.Exp = 1000;
        allDatabase.myInfo.Bread = 100;
        allDatabase.myInfo.Gold = 3000;
        */

     



        //allDatabase.allCharacter = allCharacterList;
        //allDatabase.allWeapon = allWeaponList;

        allDatabase.allCharacter = MyCharacterList;
        allDatabase.allWeapon = MyWeaponList;

        //allDatabase.allArmor = allArmorList;
        allDatabase.allArmor = MyArmorList;
        allDatabase.myInfo = myInfo;
        string jdata = JsonUtility.ToJson(allDatabase); //직렬화
        File.WriteAllText(Application.dataPath + "/Resources/MyAllDatabase.txt", jdata);

    }

    void Load()
    {

        string jdata = File.ReadAllText(Application.dataPath + "/Resources/MyAllDatabase.txt");
        AllDatabase allDatabase = JsonUtility.FromJson<AllDatabase>(jdata); //역직렬화
        myInfo = allDatabase.myInfo;
        MyCharacterList = allDatabase.allCharacter;
        MyWeaponList = allDatabase.allWeapon;
        MyArmorList = allDatabase.allArmor;

        LoadSprites();

    }


    //메모리 효율성을 위해, My..List의 리소스들만 메모리에 올리고 필요한 스프라이트는 필요할 때 따로 올린다.
    void LoadSprites()
    {
        // 캐릭터 스프라이트 로드
        CharacterSprite = new Sprite[MyCharacterList.Count];
        for (int i = 0; i < MyCharacterList.Count; i++)
        {
            CharacterSprite[i] = Resources.Load<Sprite>("Character/" + MyCharacterList[i].Name);
        }

        // 무기 스프라이트 로드
        WeaponSprite = new Sprite[MyWeaponList.Count];
        for (int i = 0; i < MyWeaponList.Count; i++)
        {
            WeaponSprite[i] = Resources.Load<Sprite>("Weapon/" + MyWeaponList[i].Name);
        }

        // 방어구 스프라이트 로드
        ArmorSprite = new Sprite[MyArmorList.Count];
        for (int i = 0; i < MyArmorList.Count; i++)
        {
            ArmorSprite[i] = Resources.Load<Sprite>("Armor/" + allArmorList[i].Name);
        }

        // 직업 스프라이트 로드 (JobSprite 배열의 크기는 JobType enum의 크기와 같아야 한다)
        JobSprite = new Sprite[Enum.GetNames(typeof(JobType)).Length];
        for (int i = 0; i < JobSprite.Length; i++)
        {
            JobSprite[i] = Resources.Load<Sprite>("Job/" + ((JobType)i).ToString());
        }

        // 속성 스프라이트 로드 (ElementSprite 배열의 크기는 Element enum의 크기와 같아야 한다)
        ElementSprite = new Sprite[Enum.GetNames(typeof(Element)).Length];
        for (int i = 0; i < ElementSprite.Length; i++)
        {
            ElementSprite[i] = Resources.Load<Sprite>("Element/" + ((Element)i).ToString());
        }
    }

    // MyCharacterList의 각 요소가 해당 MyWeaponList의 요소를 참조
    void ReconnectEquipmentReferences()
    {

        foreach (var character in MyCharacterList)
        {
            //무기 참조
            if (character.EquippedWeapon != null && character.EquippedWeapon.Name != "")
            {
                Weapon foundWeapon = MyWeaponList.FirstOrDefault(weapon => weapon.Name == character.EquippedWeapon.Name);
                if (foundWeapon != null)
                {
                    character.EquippedWeapon = foundWeapon;
                }
                else
                {
                    Debug.LogError("Weapon named " + character.EquippedWeapon.Name + " not found in MyWeaponList.");
                }
            }

            //방어구 참조
            if (character.EquippedArmor != null && character.EquippedArmor.Name != "")
            {
                Armor foundArmor = MyArmorList.FirstOrDefault(armor => armor.Name == character.EquippedArmor.Name);
                if (foundArmor != null)
                {
                    character.EquippedArmor = foundArmor;
                }
                else
                {
                    Debug.LogError("Armor named " + character.EquippedArmor.Name + " not found in MyArmorList.");
                }
            }
        }
    }

}







