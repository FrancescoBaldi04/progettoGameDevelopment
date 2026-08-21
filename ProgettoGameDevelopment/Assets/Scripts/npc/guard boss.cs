using UnityEngine;

public class guardboss : Nemico
{
	public float targetDistance=3.0f;
	private float timer=1.0f;
	private bool isDying=false;
	public int HitPoints=300;
	void Start() {
		up=isFree(Vector2.up);
		down=isFree(Vector2.down);
		left=isFree(Vector2.left);
		right=isFree(Vector2.right);
		if (parassita.StatoAttuale==Parassita.Stato.possessing) {	
			StatoAttuale=Stato.positioning;
		} else {
			StatoAttuale=Stato.waiting;
		}
	}
	
	void Update() {
		if (HitPoints<=0 && !isDying) {
			isDying = true;
			Destroy(gameObject, 1.5f);
		}
		switch (StatoAttuale) {
			
			case Stato.waiting: {
				if (parassita.StatoAttuale==Parassita.Stato.possessing) {
					this.StatoAttuale=Stato.positioning;
				}
			break;
			}
		
			case Stato.positioning: {
				if (parassita.StatoAttuale==Parassita.Stato.libero) {
					this.StatoAttuale=Stato.waiting;
					break;
				}
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
				timer-=Time.deltaTime;
				Vector2 myPosition=transform.position;
				Vector2 parassitaPosition=parassita.transform.position;
				float distance=Vector2.Distance(myPosition, parassitaPosition);
				Vector2 directionToParassita=(parassitaPosition-myPosition).normalized;
				int obstacleLayerMask=LayerMask.GetMask("ground");
				RaycastHit2D hit=Physics2D.Raycast(myPosition, directionToParassita, distance, obstacleLayerMask);
				if (hit.collider!=null) {
					this.StatoAttuale=Stato.positioning;
					timer=1.0f;
				} else if (timer<=0) {
					Shoot();
					timer=1.0f;
				}
			break;
			}
		}
	}
	
	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.name == "bullet") {
			PrendiDanno(10);
		}
	}
}








