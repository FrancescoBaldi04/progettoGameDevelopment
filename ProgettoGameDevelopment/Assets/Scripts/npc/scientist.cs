using UnityEngine;

public class scientist : Nemico
{
	private bool isDying=false;
	
	void Start() {
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

	void Update() {
		if (HitPoints<=0 && !isDying) {
			isDying = true;
			Destroy(gameObject, 1.5f);
		}
		Movement parassitaMovement=parassita.GetComponent<Movement>();
		switch (StatoAttuale)
		{
			case Stato.catching: {
				Movement movement=GetComponent<Movement>();
				Vector2 VersoDiCattura=((Vector2)parassita.transform.position-(Vector2)transform.position).normalized;
				movement.SetDirection(VersoDiCattura);
				if (parassita.StatoAttuale==Parassita.Stato.possessing) {
					this.StatoAttuale=Stato.escaping;
				}
			break;
			}
		
			case Stato.escaping: {
				Movement movement=GetComponent<Movement>();
				Vector2 VersoDiFuga=((Vector2)transform.position-(Vector2)parassita.transform.position).normalized;
				movement.SetDirection(VersoDiFuga);
				if (parassita.StatoAttuale==Parassita.Stato.libero) {
					this.StatoAttuale=Stato.catching;
				}
			break;
			}
		
			case Stato.possessed: {
				if (parassita.StatoAttuale==Parassita.Stato.libero) 
					{
						this.HitPoints=0;
					}
			break;
			}
		}
	}
	
	private void OnCollisionEnter2D(Collision2D collision) {
		if (this.StatoAttuale==Stato.catching) 
		{
			if (collision.gameObject.name == "parassita" && parassita.StatoAttuale==Parassita.Stato.possessing) 
				{
					Debug.Log("è stato posseduto");
					this.StatoAttuale=Stato.possessed;
				} else {
					Debug.Log("parassita catturato");
					//come chiamamo il metodo per la schermata di game over?
			}
		}
	}
}








