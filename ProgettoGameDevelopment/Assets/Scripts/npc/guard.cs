using UnityEngine;
 
public class guard : Nemico
{
	public float targetDistance=3.0f;
	private float timer=1.0f;
	private bool isDying=false;
	private Movement movement;
	private int obstacleLayerMask;

    protected override void Awake()
    {
        base.Awake();

		movement = GetComponent<Movement>();
		obstacleLayerMask = LayerMask.GetMask("ground");
    }

	void Start() {
		
		if (parassita.StatoAttuale==Parassita.Stato.possessing) {	
			StatoAttuale=Stato.positioning;
		} else {
			StatoAttuale=Stato.escaping;
		}
	}
	
	void Update() {
		if (isDying) return;
		
		if (HitPoints<=0) {
			Die();
			return;
		}

		switch (StatoAttuale) {
			case Stato.escaping: {
				Vector2 VersoDiFuga=((Vector2)transform.position-(Vector2)parassita.transform.position).normalized;
				movement.SetDirection(VersoDiFuga);
				
				if (parassita.StatoAttuale==Parassita.Stato.possessing) {
					this.StatoAttuale=Stato.positioning;
				}
				
				break;
			}
		
			case Stato.positioning: {
				if (parassita.StatoAttuale==Parassita.Stato.libero) {
					this.StatoAttuale=Stato.escaping;
					break;
				}
				
				if (movement!=null && parassita!=null) {
					Vector2 myPosition=transform.position;
					Vector2 parassitaPosition=parassita.transform.position;
					
					float distance=Vector2.Distance(myPosition, parassitaPosition);
					
					Vector2 directionToParassita=(parassitaPosition-myPosition).normalized;

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
			
			case Stato.possessed: {
				if (parassita.StatoAttuale == Parassita.Stato.libero) 
				{
					this.HitPoints = 0;
					break;
				}

				// Leggo l'input da InputManager e lo passo al Movement dell'Npc
				Vector2 inputGiocatore = InputManager.movement;
				movement.SetDirection(inputGiocatore);
				UpdateAnimation(inputGiocatore);

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
		if (collision.gameObject.CompareTag("Bullet")) {
			PrendiDanno(10);
			return;
		}
		
		// In questo caso vogliamo che appena la guardia tocca il parassita mentre il parassita non è in aria lo uccida? 
		// per ora non succede niente ma in caso basta copiare la seconda parte della funzione di scientist per modificarlo
	}

	private void UpdateAnimation(Vector2 direction)
	{
		// inserire qui la logica delle animazioni delle guardie 
	}

}








