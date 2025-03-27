using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static private Main S;
    static private Dictionary<eWeaponType, WeaponDefinition> WEAP_DICT;

    [Header("Inscribed")]
    public bool                 spawnEnemies = true;
    public GameObject[]         prefabEnemies;
    public float                enemySpawnPerSecond = 0.5f;
    public float                enemyInsetDefault = 1.5f;
    public float                gameRestartDelay = 2;
    public WeaponDefinition[]   weaponDefinitions;

    private BoundsCheck bndCheck;

    void Awake()
    {
        S = this;
        bndCheck = GetComponent<BoundsCheck>();

        Invoke(nameof(SpawnEnemy), 1f/enemySpawnPerSecond);

        WEAP_DICT = new Dictionary<eWeaponType, WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions){
            WEAP_DICT[def.type] = def;
        }
    }

    public void SpawnEnemy(){
        if (!spawnEnemies){
            Invoke(nameof(SpawnEnemy), 1f/enemySpawnPerSecond);
            return;
        }
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

        float enemyInset = enemyInsetDefault;
        if (go.GetComponent<BoundsCheck>() != null){
            enemyInset = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + enemyInset;
        float xMax = bndCheck.camWidth - enemyInset;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyInset;
        go.transform.position = pos;

        Invoke(nameof(SpawnEnemy), 1f/enemySpawnPerSecond);
    }

    void DelayedRestart() {
        Invoke(nameof(Restart), gameRestartDelay);
    }

    void Restart(){
        SceneManager.LoadScene("__Scene_0");
    }

    static public void HERO_DIED(){
        S.DelayedRestart();
    }

    /// <summary>
    /// Static function that gets a WeaponDefinition from the WEAP_DICT static protected field of the Main class.
    /// </summary>
    /// <returns>The WeaponDefinition, or if there is no WeaponDefinition with the eWeaponType passed in, returns a new WeaponDefinition with a eWeaponType 
    ///     of eWeaponType.none.</return>
    /// <param name="wt">The eWeaponType of the desired WeaponDefinition</param>
    static public WeaponDefinition GET_WEAPON_DEFINITION(eWeaponType wt) {
        if(WEAP_DICT.ContainsKey(wt)){
            return WEAP_DICT[wt];
        }
        // If no entry of the correct type exists in WEAP_DICT, return a new WeaponDefinition with a type of eWeaponType.none (the default value)
        return new WeaponDefinition();
    }
}
