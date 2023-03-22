using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour // ties 3:00:00 uzsibaige sita vieta
{
    public static GameManager instance;

    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(hud);
            Destroy(menu);
            return;
        }

        //test , jei nori kad nuimtu coins value nuo GameManager. Man neveikia sitas (2:30:00)
        //PlayerPrefs.DeleteAll();

        instance = this;
        SceneManager.sceneLoaded += LoadState;
        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }
    //Resources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;

    //References
    public Player player;
    public Weapon weapon;
    public FloatingTextManager floatingTextManager;
    public RectTransform hitpointBar;
    public GameObject hud;
    public GameObject menu;
    public Animator deathMenuAnim;

    //// Logic
    public int coins;
    public int experience;

    //// Floating text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    //upgrade weapon
    public bool TryUpgradeWeapon()
    {
        // is the weapon max level?
        if (weaponPrices.Count <= weapon.weaponLevel)
            return false;

        if (coins >= weaponPrices[weapon.weaponLevel])
        {
            coins -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }

        return false;
    }

    // Hitpoin Bar
    public void OnHitpointChange()
    {
        float ratio = (float)player.hitpoint / (float)player.maxHitpoint;
        hitpointBar.localScale = new Vector3(ratio, 1, 1);
    }

    // Experience system
    public int GetCurrentLevel()
    {
        int r = 0;
        int add = 0;
         while (experience >= add)  // tikrina kokio lvl esi ir returnins tavo lvl.
        {
            add += xpTable[r];
            r++;

            if (r == xpTable.Count) // max level
                return r;
        }
        return r;
    }
    public int GetXpToLevel(int level)
    {
        int r = 0;
        int xp = 0;

        while (r< level)
        {
            xp += xpTable[r];
            r++;
        }

        return xp;
    }
    public void GrantXp(int xp)
    {
        int currentLevel = GetCurrentLevel();
        experience += xp;
        if (currentLevel < GetCurrentLevel())
            OnLevelUp();
    }
    public void OnLevelUp()
    {
        Debug.Log("Level Up");
        player.OnLevelUp();
    }
    // Save state
    /*
     * Int preferedSkin
     * Int coins
     *Int experience
     *Int weaponLevel
     *
     * 
     */

    // On Scene Loaded
    public void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }
    public void SaveState()
    {
        // Debug.Log("SaveState"); nereikia
        string s = "";
        s += "0" + "|";
        s += coins.ToString() + "|";
        s += experience.ToString() + "|";
        s += weapon.weaponLevel.ToString();


        PlayerPrefs.SetString("SaveState", s);
    }

    // Death Menu and Respawn
    public void Respawn()
    {
        /*GameManager.instance.*/deathMenuAnim.SetTrigger("Hide");
        //deathMenuAnim.SetTrigger("Hide");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main"); // patriggerina "Hide" kuris animatoriuje padaro perejima prie menu-hidden.
        player.Respawn();
    }

    // Load state
    public void LoadState(Scene s, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= LoadState;
        if (!PlayerPrefs.HasKey("SaveState"))
            return;

        //                SceneManager.sceneLoaded -= LoadState;
        string[] data = PlayerPrefs.GetString("SaveState").Split('|');
        //   "0|10|15|2", splitina i 0, 10, 15, 2 atskirus strings

        //Change player skin [VELIAU]
        coins = int.Parse(data[1]);

        //Experience     
        experience = int.Parse(data[2]);
        if (GetCurrentLevel() != 1)
            player.SetLevel(GetCurrentLevel());

        // Change the weapon level [VELIAU]
        weapon.SetWeaponLevel(int.Parse(data[3]));

       // Debug.Log("LoadState");  nebereikia
        //player.transform.position = GameObject.Find("SpawnPoint").transform.position; nebereikia
    }

}
