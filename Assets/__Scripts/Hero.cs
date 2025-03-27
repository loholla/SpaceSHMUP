using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero  S { get; private set;}
    [Header("Inscribed")]

    public float        speed = 30;
    public float        rollMult = -45;
    public float        pitchMult = 30;
    public GameObject   projectilePrefab;
    public float        projectileSpeed = 40;
    public Weapon[]     weapons;

    [Header("Dynamic")] [Range(0,4)]
    private float       _shieldLevel = 1;

    [Tooltip("This field holds a reference to the last triggering GameObject")]
    private GameObject  lastTriggerGo = null;

    public delegate void WeaponFireDelegate();

    public event WeaponFireDelegate fireEvent;

    void Awake() {
        if (S == null) {
            S = this;
        } else {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");
        }
        // fireEvent += TempFire;
    }

    void Update() {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += hAxis * speed * Time.deltaTime;
        pos.y += vAxis * speed * Time.deltaTime;
        transform.position = pos;

        transform.rotation = Quaternion.Euler(vAxis*pitchMult,hAxis*rollMult,0);

        // if(Input.GetKeyDown(KeyCode.Space)){
        //     TempFire();
        // }

        if (Input.GetAxis("Jump") == 1 && fireEvent != null){
            fireEvent();
        }
    }

    // void TempFire() {
    //     GameObject projGO = Instantiate<GameObject>(projectilePrefab);
    //     projGO.transform.position = transform.position;
    //     Rigidbody rigidb = projGO.GetComponent<Rigidbody>();
    //     // rigidb.linearVelocity = Vector3.up * projectileSpeed;

    //     ProjectileHero proj = projGO.GetComponent<ProjectileHero>();
    //     float tSpeed = Main.GET_WEAPON_DEFINITION(proj.type).velocity;
    //     rigidb.linearVelocity = Vector3.up * tSpeed;
    // }

    void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        if(go == lastTriggerGo) return;
        lastTriggerGo = go;

        Enemy enemy = go.GetComponent<Enemy>();
        PowerUp pUp = go.GetComponent<PowerUp>();
        if (enemy != null){
            shieldLevel--;
            Destroy(go);
        } else if (pUp != null) {
            AbsorbPowerUp(pUp);
        } else {
            Debug.LogWarning("Shield trigger hit by non-Enemy: "+go.name);
        }
    }

    public void AbsorbPowerUp(PowerUp pUp){
        Debug.Log("Absorbed PowerUp: " + pUp.type);
        switch (pUp.type) {
            case eWeaponType.shieldLevel:
                shieldLevel++;
                break;
            default:
                if (pUp.type == weapons[0].type){
                    Weapon weap = GetEmptyWeaponsSlot();
                    if (weap != null){
                        weap.SetType(pUp.type);
                    }
                } else {
                    ClearWeapons();
                    weapons[0].SetType(pUp.type);
                }
                break;
        }
        pUp.AbsorbedBy(this.gameObject);
    }

    public float shieldLevel{
        get {return (_shieldLevel);}
        private set{
            _shieldLevel = Mathf.Min(value, 4);
            if (value < 0) {
                Destroy(this.gameObject);
                Main.HERO_DIED();
            }
        }
    }

    Weapon GetEmptyWeaponsSlot(){
        for (int i=0; i < weapons.Length; i++){
            if (weapons[i].type == eWeaponType.none) {
                return weapons[i];
            }
        }
        return null;
    }

    void ClearWeapons() {
        foreach (Weapon w in weapons){
            w.SetType(eWeaponType.none);
        }
    }
}
