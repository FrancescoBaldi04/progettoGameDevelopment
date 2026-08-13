using UnityEngine;
using UnityEngine.InputSystem;

public class Parassita : MonoBehaviour
{
    public enum Stato {libero, possessing};
    private GameObject corpoPosseduto;
    private Stato statoAttuale; // pensare se lasciare static o no, non sono sicuro
    public Stato StatoAttuale => statoAttuale; // proprietà in sola lettura, è il modo breve e compatto per scrivere un getter solo che invece della funzione rende accessibile la variabile di un oggetto di questa classe in sola lettura
    private bool hasTrojanHorse= false;
    private bool hasZipBomb =false;
    private bool hasWorm = false;
    private bool running = false;
    private float timerConsumo;
    private float raggioEsplosione = 5f;
    //private int dannoEsplosione = 50;
[SerializeField] public float moveSpeed = 1.5f;
    private Rigidbody2D rb; 
    private Animator animator;
    private Vector2 movement;
    private const string horizontal = "Horizontal"; // nomi parametri float che ho usato nell'animator
    private const string vertical = "Vertical"; 
    private const string lastHorizontal = "LastHorizontal";
    private const string lastVertical = "LastVertical";
   private Parassita parassita;
   private PlayerJump playerJump;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
         parassita = GetComponent<Parassita>();
        playerJump = GetComponent<PlayerJump>();
       statoAttuale = Stato.libero;
       timerConsumo = 0; 
    }

    // Update is called once per frame
    void Update()
    { 
        if (playerJump != null && (playerJump.isInAir || playerJump.isDead)) {
            return;
        }
        
        movement = InputManager.movement;
        
        animator.SetFloat(horizontal, movement.x);
        animator.SetFloat(vertical, movement.y);

        if (movement != Vector2.zero) {
            animator.SetFloat(lastHorizontal, movement.x);
            animator.SetFloat(lastVertical, movement.y);
        }

        if (playerJump != null && playerJump.isCharging){
            rb.linearVelocity = Vector2.zero;  // se sta caricando il salto blocco il movimento 
        }else{
            rb.linearVelocity = moveSpeed * movement;
        }

        if (Keyboard.current.eKey.wasPressedThisFrame) {
            parassita.EsplosioneZipBomb();
        }
        if(StatoAttuale == Stato.possessing)
        {
            ConsumoPossesso();
        }
        if (Keyboard.current.cKey.wasPressedThisFrame)
{
    Run();
}
    }
public void Possiedi(GameObject corpo)
    {
        corpoPosseduto = corpo;

        statoAttuale = Stato.possessing;
          timerConsumo=60;
        
    }

    public void SubisciDanno(int danno)
    {
        if(StatoAttuale == Stato.possessing)
        {
              timerConsumo = timerConsumo-danno;

            if( timerConsumo<= 0)
            {
                
            }
        }

    }

private void ConsumoPossesso(){
    timerConsumo += Time.deltaTime;

    if (timerConsumo >= 1f  )
    {if(hasTrojanHorse){
        timerConsumo -= 1;
        
    }else{
        timerConsumo -= 2;
    }
    }
}

 
 public void EsplosioneZipBomb(){

    if(!hasZipBomb){
        
        Debug.Log("Zip Bomb non sbloccata");
        return;
    }
    if(statoAttuale != Stato.possessing){
    Debug.Log("Zip Bomb non disponibile");
        return;
    }
    Debug.Log("BOOM! Zip Bomb esplosa");


    // posizione dell'esplosione
    Vector3 posizione = corpoPosseduto.transform.position;


    // trova tutti i collider nel raggio
    Collider[] colpiti = Physics.OverlapSphere(
        posizione,
        raggioEsplosione
    );


    foreach(Collider c in colpiti)
    {   
        if(c.GetComponent<Parassita>() != null){
            continue;
        }

        // bisogna creare una classe padre nemici generica, così che la zip bomb prenda tutti
        // esempio provvisorio con guard:
        /*guard umano = c.GetComponent<guard>();

       if(umano != null)
        {
            umano.HitPoints -= dannoEsplosione;

            Debug.Log(
                "Danno Zip Bomb a " + umano.name
            );
        }*/
    }


    // distrugge il corpo sacrificato
    Destroy(corpoPosseduto);


    corpoPosseduto = null;

    statoAttuale = Stato.libero;   
}
public void Run (){
    if(hasWorm){
running= !running;
if(running){
moveSpeed+=2f;
}else{moveSpeed-=2f;

}

    }
}
public void UnlockTrojanHorse(){
    hasTrojanHorse = true;
    Debug.Log("Trojan Horse sbloccato!");
}
public void UnlockZipBomb(){
    hasZipBomb = true;
    Debug.Log("Zip Bomb sbloccato!");
}
public void UnlockWorm(){
    hasWorm=true;
    Debug.Log("Worm sbloccato!");
}

private void OnDrawGizmosSelected(){
    Gizmos.DrawWireSphere(
        transform.position,
        raggioEsplosione
    );
}
}

