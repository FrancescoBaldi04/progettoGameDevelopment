using UnityEngine;

public class scientist : Nemico
{
	private bool isDying=false;
	private Movement movement;

    protected override void Awake()
    {
        base.Awake();
		movement = GetComponent<Movement>();
    }

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
		if (isDying) return;
		
		if (HitPoints<=0) {
			Die();
			return;
		}

		switch (StatoAttuale)
		{
			case Stato.catching: {
				Vector2 VersoDiCattura=GetBestDirection(parassita.transform.position, Vector2.zero);
				movement.SetDirection(VersoDiCattura);
				if (parassita.StatoAttuale==Parassita.Stato.possessing) {
					this.StatoAttuale=Stato.escaping;
				}
			break;
			}
		
			case Stato.escaping: {
				Vector2 VersoDiFuga=GetBestDirection(parassita.transform.position, Vector2.zero);
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

    protected override void Die()
    {
        if (isDying) return;
		isDying = true;

		if (movement != null) movement.speed = 0f;
		Destroy(gameObject, 1.5f);
    }

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.name == "bullet") {
			PrendiDanno(10);
		}
		if (this.StatoAttuale==Stato.catching) 
		{
			/*if (collision.gameObject.name == "parassita" && parassita.StatoAttuale==Parassita.Stato.possessing) 
				{
					Debug.Log("è stato posseduto");
					this.StatoAttuale=Stato.possessed;
				} else {
					Debug.Log("parassita catturato");
					//come chiamamo il metodo per la schermata di game over?
			}*/

			if(collision.gameObject.name == "parassita")
			{
				if(parassita.StatoAttuale == Parassita.Stato.possessing)
				{
					Debug.Log("è stato posseduto");
					this.StatoAttuale=Stato.possessed;
				}
				else
				{
					Debug.Log("parassita catturato");
                	// Qui ora puoi rimettere in sicurezza il Game Over
				}
			}

		}
	}
}








