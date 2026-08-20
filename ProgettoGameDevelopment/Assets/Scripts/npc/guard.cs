using UnityEngine;
 
public class guard : Nemico
{
	public float targetDistance=3.0f;
	private bool isDying=false;
	void Start() {
		up=isFree(Vector2.up);
		down=isFree(Vector2.down);
		left=isFree(Vector2.left);
		right=isFree(Vector2.right);
		if (parassita.StatoAttuale==Parassita.Stato.possessing) {	
			StatoAttuale=Stato.positioning;
		} else {
			StatoAttuale=Stato.escaping;
		}
	}

	void Update() {
		if (HitPoints<=0 && !isDying) {
			isDying = true;
			Destroy(gameObject, 1.5f);
		}
		switch (StatoAttuale) {
			case Stato.escaping: {
				Movement movement=GetComponent<Movement>();
				Vector2 VersoDiFuga=((Vector2)transform.position-(Vector2)parassita.transform.position).normalized;
				movement.SetDirection(VersoDiFuga);
				if (parassita.StatoAttuale==Parassita.Stato.possessing) {
					this.StatoAttuale=Stato.positioning;
				}
			break;
			}
		
			case Stato.positioning: {
				Movement movement=GetComponent<Movement>();
				if (movement!=null && parassita!=null) {
					Vector2 myPosition=transform.position;
					Vector2 parassitaPosition=parassita.transform.position;
					float distance=Vector2.Distance(myPosition, parassitaPosition);
					Vector2 directionToParassita=(parassitaPosition-myPosition).normalized;
					int obstacleLayerMask=LayerMask.GetMask("ground");
					RaycastHit2D hit=Physics2D.Raycast(myPosition, directionToParassita, distance, obstacleLayerMask);
					if (hit.collider!=null || distance>targetDistance+0.3f) {
						movement.SetDirection(directionToParassita);
					} else {
						movement.SetDirection(Vector2.zero);
						this.StatoAttuale=Stato.shooting;
					}
				}
			break;
			}
			
			case Stato.shooting: {
				
			break;
			}
			
			case Stato.possessed: {
				if (parassita.StatoAttuale==Parassita.Stato.libero) {
					this.HitPoints=0;
				}
			break;
			}
		}
	}
	
	private void OnCollisionEnter2D(Collision2D collision) {
		if (this.StatoAttuale==Stato.escaping) {
			if (collision.gameObject.name == "parassita") {
				Debug.Log("è stato posseduto");
				this.StatoAttuale=Stato.possessed;
			}
		}
	}
}








