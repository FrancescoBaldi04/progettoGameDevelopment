using UnityEngine;

public class scientistMiniboss : Nemico
{
	public enum Stato {escaping, catching};
	public Stato StatoAttuale { get; private set; }
	private bool isDying=false;
	[SerializeField] private Parassita parassita; // assegnare il gameObject del parassita nell'inspector una volta creato l'oggetto corrispondente!

	void Start(){
		this.HitPoints=200;
		up=isFree(Vector2.up);
		down=isFree(Vector2.down);
		left=isFree(Vector2.left);
		right=isFree(Vector2.right);
		if (parassita.StatoAttuale==Parassita.Stato.possessing) {	
			StatoAttuale=Stato.escaping;
		} else {
			StatoAttuale=Stato.catching;
		}
	}

	void Update(){
		if (HitPoints<=0 && !isDying) {
			isDying = true;
			Destroy(gameObject, 1.5f);
		}
		switch (StatoAttuale)
		{
			case Stato.catching:
				Vector2 parassitaPosition=new Vector2 (parassita.transform.position.x, parassita.transform.position.y);
				Vector2 whereToGo=GetBestDirection(parassitaPosition);
				if (parassita.StatoAttuale==Parassita.Stato.possessing){
				this.StatoAttuale=Stato.escaping;
				}	
			break;
		
			case Stato.escaping:
			
				if (parassita.StatoAttuale!=Parassita.Stato.possessing)
				{
					StatoAttuale=Stato.catching;
				}
			break;
		}
	}
	
	private void OnCollisionEnter(Collision collision){
		if (this.StatoAttuale==Stato.catching) 
		{
			if (collision.gameObject.name == "parassita"){
					Debug.Log("parassita catturato");
					//come chiamamo il metodo per la schermata di game over?
			}
		}
	}
	
	
}








