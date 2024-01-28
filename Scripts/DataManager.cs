using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;



public class DataManager : MonoBehaviour
{
    public TextAsset CharacterDB;
    public TextAsset WeaponDB;
    public List<Character> allCharacterList; //ĳ���� �⺻ DB
    public List<Character> MyCharacerList; //�� ĳ���� DB
    public List<Weapon> allWeaponList; //���� �⺻ DB
    public List<Weapon> MyWeaponList; //�� ���� DB


    void Start()
    {
        string[] line_0 = CharacterDB.text.Substring(0, CharacterDB.text.Length - 1).Split('\n'); //CharacterDB(�⺻ ĳ���� ����) �� ������ �޾Ƽ� ����
        string[] line_1 = WeaponDB.text.Substring(0, WeaponDB.text.Length - 1).Split('\n'); // WeaponDB(�⺻ ���� ����) �� ������ �޾Ƽ� ����
        for (int i = 0; i < line_0.Length; i++)
        {
            string[] row = line_0[i].Split('\t'); //CharacterDB �� ������ �޾Ƽ� ����
            allCharacterList.Add(new Character(row[0],row[1],row[2],row[3],row[4],row[5]));
        }
        for (int i = 0; i < line_1.Length; i++)
        {
            string[] row = line_1[i].Split('\t'); //WeaponDB �� ������ �޾Ƽ� ����
            allWeaponList.Add(new Weapon(row[0],row[1],row[2]));
        }

        Load();

        print(MyWeaponList[2].Type);
    }

    void Save()
    {
        AllDatabase allDatabase = new AllDatabase();
        allDatabase.allCharacter = allCharacterList;
        allDatabase.allWeapon = allWeaponList;

        string jdata = JsonUtility.ToJson(allDatabase); //����ȭ
        File.WriteAllText(Application.dataPath + "/Resources/MyAllDatabase.txt", jdata);

    }

    void Load()
    {

        string jdata = File.ReadAllText(Application.dataPath + "/Resources/MyAllDatabase.txt");
        AllDatabase allDatabase = JsonUtility.FromJson<AllDatabase>(jdata); //������ȭ
        MyCharacerList = allDatabase.allCharacter;
        MyWeaponList = allDatabase.allWeapon;

    }
}







