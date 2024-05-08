using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;


public class DataModel : MonoBehaviour
{
    public static DataModel instance = null;

    public Action OnSaveRequested; //Save�� ���������� �����ϴ� ���� �����ϱ� ���� Save�̺�Ʈ

    public DataModel()
    {
        OnSaveRequested = Save; // Delegate�� Save �޼��� ����
    }

    [SerializeField] TextAsset CharacterDB;
    [SerializeField] TextAsset WeaponDB;
    [SerializeField] TextAsset ArmorDB;

    public MyInfo myInfo = new MyInfo();

    public List<Character> allCharacterList; //ĳ���� �⺻ DB
    public List<Character> MyCharacterList; //�� ĳ���� DB

    public List<Weapon> allWeaponList; //���� �⺻ DB
    public List<Weapon> MyWeaponList; //�� ���� DB

    public List<Armor> allArmorList; //�� �⺻ DB
    public List<Armor> MyArmorList; //�� �� DB



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
        string[] line_0 = CharacterDB.text.Substring(0, CharacterDB.text.Length - 1).Split('\n'); //CharacterDB(�⺻ ĳ���� ����) �� ������ �޾Ƽ� ����
        string[] line_1 = WeaponDB.text.Substring(0, WeaponDB.text.Length - 1).Split('\n'); // WeaponDB(�⺻ ���� ����) �� ������ �޾Ƽ� ����
        string[] line_2 = ArmorDB.text.Substring(0, ArmorDB.text.Length - 1).Split('\n'); // ArmorDB(�⺻ �� ����) �� ������ �޾Ƽ� ����
        for (int i = 0; i < line_0.Length; i++) 
        {
            string[] row = line_0[i].Split('\t'); //CharacterDB �� ������ �޾Ƽ� ����
            allCharacterList.Add(new Character(row[0], row[1], row[2], row[3], row[4], row[5], row[6] ,row[7], row[8]));
        }
        for (int i = 0; i < line_1.Length; i++)
        {
            string[] row = line_1[i].Split('\t'); //WeaponDB �� ������ �޾Ƽ� ����
            allWeaponList.Add(new Weapon(row[0], row[1], row[2]));
        }
        for (int i = 0; i < line_2.Length; i++)
        {
            string[] row = line_2[i].Split('\t'); //ArmorDB �� ������ �޾Ƽ� ����
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
        allDatabase.myInfo.nickName = "����";
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
        string jdata = JsonUtility.ToJson(allDatabase); //����ȭ
        File.WriteAllText(Application.dataPath + "/Resources/MyAllDatabase.txt", jdata);

    }

    void Load()
    {

        string jdata = File.ReadAllText(Application.dataPath + "/Resources/MyAllDatabase.txt");
        AllDatabase allDatabase = JsonUtility.FromJson<AllDatabase>(jdata); //������ȭ
        myInfo = allDatabase.myInfo;
        MyCharacterList = allDatabase.allCharacter;
        MyWeaponList = allDatabase.allWeapon;
        MyArmorList = allDatabase.allArmor;

        LoadSprites();

    }


    //�޸� ȿ������ ����, My..List�� ���ҽ��鸸 �޸𸮿� �ø��� �ʿ��� ��������Ʈ�� �ʿ��� �� ���� �ø���.
    void LoadSprites()
    {
        // ĳ���� ��������Ʈ �ε�
        CharacterSprite = new Sprite[MyCharacterList.Count];
        for (int i = 0; i < MyCharacterList.Count; i++)
        {
            CharacterSprite[i] = Resources.Load<Sprite>("Character/" + MyCharacterList[i].Name);
        }

        // ���� ��������Ʈ �ε�
        WeaponSprite = new Sprite[MyWeaponList.Count];
        for (int i = 0; i < MyWeaponList.Count; i++)
        {
            WeaponSprite[i] = Resources.Load<Sprite>("Weapon/" + MyWeaponList[i].Name);
        }

        // �� ��������Ʈ �ε�
        ArmorSprite = new Sprite[MyArmorList.Count];
        for (int i = 0; i < MyArmorList.Count; i++)
        {
            ArmorSprite[i] = Resources.Load<Sprite>("Armor/" + allArmorList[i].Name);
        }

        // ���� ��������Ʈ �ε� (JobSprite �迭�� ũ��� JobType enum�� ũ��� ���ƾ� �Ѵ�)
        JobSprite = new Sprite[Enum.GetNames(typeof(JobType)).Length];
        for (int i = 0; i < JobSprite.Length; i++)
        {
            JobSprite[i] = Resources.Load<Sprite>("Job/" + ((JobType)i).ToString());
        }

        // �Ӽ� ��������Ʈ �ε� (ElementSprite �迭�� ũ��� Element enum�� ũ��� ���ƾ� �Ѵ�)
        ElementSprite = new Sprite[Enum.GetNames(typeof(Element)).Length];
        for (int i = 0; i < ElementSprite.Length; i++)
        {
            ElementSprite[i] = Resources.Load<Sprite>("Element/" + ((Element)i).ToString());
        }
    }

    // MyCharacterList�� �� ��Ұ� �ش� MyWeaponList�� ��Ҹ� ����
    void ReconnectEquipmentReferences()
    {

        foreach (var character in MyCharacterList)
        {
            //���� ����
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

            //�� ����
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







