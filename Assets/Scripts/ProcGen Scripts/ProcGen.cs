// Procedural Generation Sudo-Code

//Started on 2015-08-31 by Andrew Lonsway

//Goal: To create a way to procedurally generate the world which the player will encounter, and balance performance

using UnityEngine;
using System.Collections;


public class ProcGen : MonoBehaviour {
	private float time;
	private float timesAccessed = 0;
	private ArrayList frozen;
	public GameObject asterioid;
	public GameObject small_ship;
	public GameObject large_ship;
	public GameObject small_debris;
	public GameObject med_debris;
	public GameObject large_debris;
	public GameObject small_loot;
	public GameObject med_loot;
	public GameObject large1_loot;
	public GameObject large2_loot;
	public GameObject large3_loot;
	public GameObject oxyStation;

	public GameObject laser;
	public GameObject emp;
	public GameObject airjet;
	
	public GameObject chaser;
	public GameObject spinturret;
	public GameObject EMPTurret;
	public GameObject DMGTurret;
	
	//chance values must be small, at least .05 for now

	public float asteroid_chance;
	public float small_ship_chance;
	public float large_ship_chance;
	public float small_debris_chance;
	public float med_debris_chance;
	public float large_debris_chance;
	public float small_loot_chance;
	public float med_loot_chance;
	public float large1_loot_chance;
	public float large2_loot_chance;
	public float large3_loot_chance;
	public float OxyStation_Chance;
			
	public float inside_small_loot_chance;
	public float inside_med_loot_chance;
	public float inside_large1_loot_chance; 
	public float inside_large2_loot_chance; 
	public float inside_large3_loot_chance; 
			
	public float inside_debris_chance;
	
	public float inside_hazard_laser_chance;
	public float inside_hazard_emp_chance;
	public float inside_hazard_airjet_chance;
	
	public float inside_enemy_spinturret_chance;
	public float inside_enemy_DMGTurret_chance;
	public float inside_enemy_EMPTurret_chance;
	public float inside_enemy_chaser_chance;
	

	public float density;  //density of each builds
	public float initial_distance; //how big we spawn the beginning

	//The EdgeBoundaries
	public Edgebounds topEdge;
	public Edgebounds bottomEdge;
	public Edgebounds rightEdge;
	public Edgebounds leftEdge;
	private bool isGenerating;
	GameObject topNewEdge;

	private float distanceLootChance;
	
	void Start() {
		frozen = new ArrayList();
		isGenerating = true;
		//we load up the initial level, which will consist of the player, the ship, the guitext, a random GameObject containing this script, and the camera.
		//camera has the focus set to the player, the ship has the focus set to the player
		Generate (0, 0); //generate a square of width = initial_distance * 2;
		Generate (initial_distance * 2, 0);
		Generate (initial_distance * 2, initial_distance * 2);
		Generate (0, initial_distance * 2);
		Generate (initial_distance * -2, 0);
		Generate (initial_distance * -2, initial_distance * -2);
		Generate (0, initial_distance * -2);
		Generate (initial_distance * 2, initial_distance * -2);
		Generate (initial_distance * -2, initial_distance * 2);

		//Create the EdgeCollider2D boundaries for the initial area
		topEdge.initialDistance = initial_distance;

//		topEdge.GetComponent<EdgeCollider2D> ().isTrigger = true;
//		topEdge.GetComponent<Transform> ().position = new Vector3 (0, initial_distance, 0);
//		topEdge.GetComponent<Transform> ().rotation = new Quaternion (0, 0, 90, 0);
		topEdge = (Edgebounds)Instantiate (topEdge,new Vector3(0,initial_distance,0),new Quaternion(0,0,0,0));
		bottomEdge = (Edgebounds)Instantiate (bottomEdge,new Vector3(0,-initial_distance,0),new Quaternion(0,0,0,0));
		bottomEdge.transform.Rotate (new Vector3 (0, 0, 180));
		leftEdge = (Edgebounds)Instantiate (leftEdge,new Vector3(-initial_distance,0,0),new Quaternion(0,0,0,0));
		leftEdge.transform.Rotate (new Vector3 (0, 0, 90));
		rightEdge = (Edgebounds)Instantiate (rightEdge,new Vector3(initial_distance,0,0),new Quaternion(0,0,0,0));
		rightEdge.transform.Rotate (new Vector3 (0, 0, 270));
		isGenerating = false;
	}

	public void Generate(float originX, float originY) { // generate from (startX, startY) to (endX, endY)
		Vector2 newitembuffer = Vector2.zero;
		int numspawned = 0;
		if (!isGenerating) timesAccessed = Vector3.Distance(GameObject.Find("Player").transform.position, this.transform.position)/50;
		else timesAccessed = 1;
		//print(timesAccessed);
		while (numspawned < initial_distance * density/(timesAccessed * 10)) {
			float i = initial_distance * Random.value * (Random.value > .5f?-1:1) + originX;
			float j = initial_distance * Random.value * (Random.value > .5f?-1:1) + originY;
			//Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(i,j), 4); 
			bool flag = false;
			/*foreach (Collider2D c in hitColliders) {
				if (c.GetComponent<SpawnVals>() != null) {
					flag = true;
				}
			}*/

			if (originX == 0 && originY == 0) {
				if ((i > 13 || i < -13) && (j > 13 || j < -13)) { //not colliding into ship
					newitembuffer = FindToGenerate(i, j);
					j+=newitembuffer.y;
					i+=newitembuffer.x;
					numspawned++;
				}
			}else{
				newitembuffer = FindToGenerate(i, j);
				j+=newitembuffer.y;
				i+=newitembuffer.x;
				numspawned++;
			}


		}
		/*for (float i = startX; i < endX; i = i + density * Random.value) {
			newitembuffer.x = 0;
			for (float j = startY; j < endY; j = j + density * Random.value) {
				i += 10 * Random.value * (Random.value > .5f?-1:1);
				if ((i > 12 || i < -11) && (j > 11 || j < -11)) { //not colliding into ship
					newitembuffer = FindToGenerate(i, j);
					j+=newitembuffer.y;
					i+=newitembuffer.x;
				}
			}
		}*/
	}
	void SpawnInside(GameObject g) {
			/*	Each prefab will have a script called "SpawnVals" and several values such as
			/	int count;
			/	bool enemies;
			/	bool hazards;
			/	and several GameObjects as spawners, with the name as spawner and a script "spawner" and rotations/positions
			/	script spawner has several values for weighted spawns such as 
			/	float hazard_chance
			/	float turret_chance
			/	float all_loot_chance
			*/
			
			Transform[] spawners = new Transform[g.GetComponent<SpawnVals>().count];
			
			int i = 0;
			
			if (g.activeInHierarchy) {
				foreach (Transform gc in g.GetComponentInChildren<Transform>()) {
					if (gc.name == "Spawner") {
						spawners [i] = gc;
						i++;
					} else if (gc.GetComponent<MultiResultStarter>()) {
						gc.GetComponent<MultiResultStarter>().poopedOut();
					}
				}
			}
			//These values are subject for later level design, so things aren't 100% random and some weighted chance will happen for challenges
			float tmp_inside_hazard_laser_chance = inside_hazard_laser_chance;
			float tmp_inside_hazard_emp_chance = inside_hazard_emp_chance;
			float tmp_inside_loot_chance = inside_small_loot_chance;
			float tmp_inside_hazard_airjet_chance = inside_hazard_airjet_chance;
			float tmp_inside_enemy_chaser_chance = inside_enemy_chaser_chance;

			bool bEnemies = g.GetComponent<SpawnVals>().enemies;
			bool bHazards = g.GetComponent<SpawnVals>().hazards;

		/*	Each item will have its own inside chance, and for some, more will spawn to take their place
			/	The chances will increase for each challenge spawned, and for some loot as well.
			/	Each item will have its own weighted chances as well from the spawner script itself
			/	So once again, the 2 factors at most will be weighted chance and spawner chance.
			*/
			foreach (Transform c in spawners) {

				//print(g.name);
			if (c != null && c.GetComponent<Spawner>()) {
				if (GameObject.Find ("Player") != null) distanceLootChance = Vector3.Distance(c.position, GameObject.Find("Player").transform.position)/3000;
				else distanceLootChance = Vector3.Distance(c.position, Vector3.zero)/1000;
				bool bRotation = !c.GetComponent<Spawner>().keepRotation;

				
					// spawn small_loot
				if (Random.value < (tmp_inside_loot_chance + inside_small_loot_chance + c.GetComponent<Spawner>().all_loot_chance + distanceLootChance)) {
						GameObject thingy = (GameObject)Instantiate(small_loot, c.position, Quaternion.identity);
						thingy.transform.position = c.position + Vector3.up * .01f;
						if (bRotation) thingy.transform.rotation = c.rotation;
						else thingy.transform.rotation = Quaternion.Euler(0,0,Random.Range(0, 360));
						thingy.transform.parent = g.transform;
						continue;
					}
					// spawn med loot
				if (Random.value < (tmp_inside_loot_chance + inside_med_loot_chance + c.GetComponent<Spawner>().all_loot_chance + distanceLootChance)) {
						GameObject thingy = (GameObject)Instantiate(med_loot, c.position, Quaternion.identity);
						thingy.transform.position = c.position + Vector3.up * .01f;
					if (bRotation) thingy.transform.rotation = c.rotation;
					else thingy.transform.rotation = Quaternion.Euler(0,0,Random.Range(0, 360));
						tmp_inside_loot_chance *= 1.5f;
						thingy.transform.parent = g.transform;

						continue;
					}
					// spawn large1 loot
				if (Random.value <  (tmp_inside_loot_chance + inside_large1_loot_chance + distanceLootChance)) {
						GameObject thingy = (GameObject)Instantiate(large1_loot, c.position, Quaternion.identity);
						thingy.transform.position = c.position + Vector3.up * .01f;
					if (bRotation) thingy.transform.rotation = c.rotation;
					else thingy.transform.rotation = Quaternion.Euler(0,0,Random.Range(0, 360));
						tmp_inside_loot_chance *= 1.5f;
					//	thingy.transform.parent = g.transform;

						continue;
					}
					// spawn large2 loot
				if (Random.value < (tmp_inside_loot_chance + inside_large2_loot_chance + distanceLootChance)) {
						GameObject thingy = (GameObject)Instantiate(large2_loot, c.position, Quaternion.identity);
						thingy.transform.position = c.position + Vector3.up * .01f;
					if (bRotation) 	thingy.transform.rotation = c.rotation;
						tmp_inside_loot_chance *= 2;
					//	thingy.transform.parent = g.transform;

						continue;
					}
					// spawn large3 loot
				if (Random.value < (tmp_inside_loot_chance + inside_large3_loot_chance + distanceLootChance)) {
						GameObject thingy = (GameObject)Instantiate(large3_loot, c.position, Quaternion.identity);
						thingy.transform.position = c.position + Vector3.up * .01f;
					if (bRotation) thingy.transform.rotation = c.rotation;
					else thingy.transform.rotation = Quaternion.Euler(0,0,Random.Range(0, 360));
						tmp_inside_loot_chance *= 3;
					//	thingy.transform.parent = g.transform;

						continue;
					}
				//spawn oxy station
				if (Random.value < (OxyStation_Chance + distanceLootChance)) {
					GameObject thingy = (GameObject)Instantiate(this.oxyStation, c.position, Quaternion.identity);
					thingy.transform.position = c.position + Vector3.up * .01f;
					if (bRotation) thingy.transform.rotation = c.rotation;
					else thingy.transform.rotation = Quaternion.Euler(0,0,Random.Range(0, 360));
					//	thingy.transform.parent = g.transform;
					
					continue;
				}
					// spawn debris chance
					if (Random.value < (inside_debris_chance)) {
						GameObject thingy = (GameObject)Instantiate(small_debris, c.position, Quaternion.identity);
						thingy.transform.position = c.position + Vector3.up * .01f;
					if (bRotation) thingy.transform.rotation = c.rotation;
					else thingy.transform.rotation = Quaternion.Euler(0,0,Random.Range(0, 360));
						thingy.transform.parent = g.transform;

						continue;
					}
					// spawn laser hazard_chance
				if (bHazards && Random.value < (tmp_inside_hazard_laser_chance + c.GetComponent<Spawner>().hazard_chance + distanceLootChance)) {
						GameObject thingy = (GameObject)Instantiate(laser, c.position, Quaternion.identity);
						thingy.transform.position = c.position + Vector3.up * .01f;
					if (bRotation || true) thingy.transform.rotation = c.rotation;
					else thingy.transform.rotation = Quaternion.Euler(0,0,Random.Range(0, 360));
						tmp_inside_hazard_laser_chance *= 5;
						thingy.transform.parent = g.transform;

						continue;
					}
					// spawn emp 
				if (bHazards && Random.value < (tmp_inside_hazard_emp_chance + c.GetComponent<Spawner>().hazard_chance + distanceLootChance)) {
						GameObject thingy = (GameObject)Instantiate(emp, c.position, Quaternion.identity);
						thingy.transform.position = c.position + Vector3.up * .01f;
					if (bRotation) 	thingy.transform.rotation = c.rotation;
						tmp_inside_hazard_emp_chance *= 5;
						thingy.transform.parent = g.transform;

						continue;
					}
					// spawn airjet
				if (bHazards && Random.value < (tmp_inside_hazard_airjet_chance + c.GetComponent<Spawner>().hazard_chance + distanceLootChance)) {
						GameObject thingy = (GameObject)Instantiate(airjet, c.position, Quaternion.identity);
						thingy.transform.position = c.position + Vector3.up * .01f;
					if (bRotation|| true) thingy.transform.rotation = c.rotation;
					else thingy.transform.rotation = Quaternion.Euler(0,0,Random.Range(0, 360));
						tmp_inside_hazard_airjet_chance *= 5;
						thingy.transform.parent = g.transform;

						continue;
					}
					// spawn spinturret
				if (bEnemies && Random.value < (inside_enemy_spinturret_chance + c.GetComponent<Spawner>().turret_chance) && inside_enemy_spinturret_chance != 0 + distanceLootChance) {
						GameObject thingy = (GameObject)Instantiate(spinturret, c.position, c.rotation);
					thingy.transform.position = c.position + Vector3.up * .01f;
					if (bRotation) thingy.transform.rotation = c.rotation;
					else thingy.transform.rotation = Quaternion.Euler(0,0,Random.Range(0, 360));
						thingy.GetComponent<PhysicsTurret>().rotationUpperLim = c.GetComponent<Spawner>().turretStart;
						thingy.GetComponent<PhysicsTurret>().rotationLowerLim = c.GetComponent<Spawner>().turretEnd;
						thingy.transform.parent = g.transform;

						continue;
					}
					// spawn dmgturret
				if (bEnemies && Random.value < (inside_enemy_DMGTurret_chance + c.GetComponent<Spawner>().turret_chance) && inside_enemy_DMGTurret_chance != 0 + distanceLootChance) {
						GameObject thingy = (GameObject)Instantiate(DMGTurret, c.position, Quaternion.identity);
						thingy.transform.position = c.position + Vector3.up * .01f;
					if (bRotation) thingy.transform.rotation = c.rotation;
					else thingy.transform.rotation = Quaternion.Euler(0,0,Random.Range(0, 360));
					//thingy.transform.rotation = c.rotation;
						thingy.GetComponent<PhysicsTurret>().rotationUpperLim = c.GetComponent<Spawner>().turretStart;
						thingy.GetComponent<PhysicsTurret>().rotationLowerLim = c.GetComponent<Spawner>().turretEnd;
						thingy.transform.parent = g.transform;

						continue;
					}
					// spawn empturret
				if (bEnemies && Random.value < (inside_enemy_EMPTurret_chance + c.GetComponent<Spawner>().turret_chance) && inside_enemy_EMPTurret_chance != 0 + distanceLootChance) {
						GameObject thingy = (GameObject)Instantiate(EMPTurret, c.position, Quaternion.identity);
						thingy.transform.position = c.position + Vector3.up * .01f;
					if (bRotation) thingy.transform.rotation = c.rotation;
					else thingy.transform.rotation = Quaternion.Euler(0,0,Random.Range(0, 360));
						//thingy.transform.rotation = c.rotation;
						thingy.GetComponent<PhysicsTurret>().rotationUpperLim = c.GetComponent<Spawner>().turretStart;
						thingy.GetComponent<PhysicsTurret>().rotationLowerLim = c.GetComponent<Spawner>().turretEnd;
						thingy.transform.parent = g.transform;

						continue;
					}
					// spawn chaser
				if (bEnemies && Random.value < (tmp_inside_enemy_chaser_chance + c.GetComponent<Spawner>().turret_chance + distanceLootChance)) {
						GameObject thingy = (GameObject)Instantiate(chaser, c.position, Quaternion.identity);
						thingy.transform.position = c.position + Vector3.up * .01f;
					if (bRotation) thingy.transform.rotation = c.rotation;
					else thingy.transform.rotation = Quaternion.Euler(0,0,Random.Range(0, 360));
						thingy.transform.parent = g.transform;

						tmp_inside_enemy_chaser_chance *= 5;
					}
				}
		}
			
	}
	GameObject instantiateVerify(GameObject g, Vector3 v, Quaternion i) {
		if (!g.GetComponent<MultiResultStarter>()) return (GameObject)GameObject.Instantiate(g, v, i);
		Collider2D[] hitColliders = Physics2D.OverlapCircleAll(v, g.GetComponent<MultiResultStarter>().radius); 
		foreach (Collider2D c in hitColliders) {
			return null;
		}
		return (GameObject)GameObject.Instantiate(g, v, i);
	}
	Vector2 FindToGenerate(float i, float j) { //separate so we can use it later on
				if (Random.value < asteroid_chance) {
					//spawn asteriod
					GameObject thingy = (GameObject)instantiateVerify(asterioid, new Vector3(i, j, 1), Quaternion.identity);
					if (thingy != null) {
						thingy = thingy.GetComponent<MultiResultStarter>().poopedOut();
						Quaternion q = new Quaternion();
						q.eulerAngles = new Vector3(0,0, Random.Range(0, 360));
						thingy.transform.rotation = q;
						SpawnInside(thingy);
					}
					return Vector2.zero;
					
				} 
				if (Random.value < small_ship_chance) {
					//spawn small ship
					GameObject thingy = (GameObject)instantiateVerify(small_ship, new Vector3(i, j, 1), Quaternion.identity);
					if (thingy != null) {
						thingy = thingy.GetComponent<MultiResultStarter>().poopedOut();	
						Quaternion q = new Quaternion();
						q.eulerAngles = new Vector3(0,0, Random.Range(0, 360));
						thingy.transform.rotation = q;
						SpawnInside(thingy);
					}
					return Vector2.zero;
				} 
				if (Random.value < large_ship_chance) {
					GameObject thingy = (GameObject)instantiateVerify(large_ship, new Vector3(i, j, 1), Quaternion.identity);
					if (thingy != null) {
					thingy = thingy.GetComponent<MultiResultStarter>().poopedOut();
					Quaternion q = new Quaternion();
					q.eulerAngles = new Vector3(0,0, Random.Range(0, 360));
					thingy.transform.rotation = q;
					SpawnInside(thingy);
					}
					return Vector2.zero;
				}
				if (Random.value < small_debris_chance) { //cannot spawn inside
			GameObject thingy = (GameObject)instantiateVerify(small_debris, new Vector3(i, j, 1), Quaternion.identity);
					if (thingy != null) {
						thingy = thingy.GetComponent<MultiResultStarter>().poopedOut();
						Quaternion q = new Quaternion();
						q.eulerAngles = new Vector3(0,0, Random.Range(0, 360));
						thingy.transform.rotation = q;
					}
					return Vector2.zero;
				}
				if (Random.value < med_debris_chance) {
			GameObject thingy = (GameObject)instantiateVerify(med_debris, new Vector3(i, j, 1), Quaternion.identity);
			if (thingy != null) {
					thingy = thingy.GetComponent<MultiResultStarter>().poopedOut();
					SpawnInside(thingy);
			}
					return Vector2.zero;
				}
				if (Random.value < large_debris_chance) {
			GameObject thingy = (GameObject)instantiateVerify(large_debris, new Vector3(i, j, 1), Quaternion.identity);
			if (thingy != null) {
					thingy = thingy.GetComponent<MultiResultStarter>().poopedOut();
					SpawnInside(thingy);
			}
					return Vector2.zero;
				}
				if (Random.value < small_loot_chance) { //cannot spawn inside, may spawn some debris around it
			GameObject thingy = (GameObject)instantiateVerify(small_loot, new Vector3(i, j, 1), Quaternion.identity);
			if (thingy != null) {
					Quaternion q = new Quaternion();
					q.eulerAngles = new Vector3(0,0, Random.Range(0, 360));
					thingy.transform.rotation = q;
			}
					return Vector2.zero;
				}
				if (Random.value < med_loot_chance) {
			GameObject thingy = (GameObject)instantiateVerify(med_loot, new Vector3(i, j, 1), Quaternion.identity);
			if (thingy != null) {
					//SpawnInside(thingy); //for multiple coins in a bunch
					Quaternion q = new Quaternion();
					q.eulerAngles = new Vector3(0,0, Random.Range(0, 360));
					thingy.transform.rotation = q;
			}
					return Vector2.zero;
				}
				if (Random.value < large1_loot_chance) {
			GameObject thingy = (GameObject)instantiateVerify(large1_loot, new Vector3(i, j, 1), Quaternion.identity);
			if (thingy != null) {
					Quaternion q = new Quaternion();
			print("spawning this loot");

					q.eulerAngles = new Vector3(0,0, Random.Range(0, 360));
					thingy.transform.rotation = q;	
			}
					return Vector2.zero;
				}
				if (Random.value < large2_loot_chance) {
			print("spawning this loot");
			GameObject thingy = (GameObject)instantiateVerify(large2_loot, new Vector3(i, j, 1), Quaternion.identity);
			if (thingy != null) {
					Quaternion q = new Quaternion();
					q.eulerAngles = new Vector3(0,0, Random.Range(0, 360));
					thingy.transform.rotation = q;
					return Vector2.zero;
			}
				}
				if (Random.value < large3_loot_chance) {
			GameObject thingy = (GameObject)instantiateVerify(large3_loot, new Vector3(i, j, 1), Quaternion.identity);
			if (thingy != null) {
					Quaternion q = new Quaternion();
			print("spawning this loot");

					q.eulerAngles = new Vector3(0,0, Random.Range(0, 360));
					thingy.transform.rotation = q;
			}
					return Vector2.zero;
				}
		return Vector2.zero;
	}

	void Update() {
		time += Time.deltaTime;
		//print (1/GameObject.Find ("Player").GetComponent<Rigidbody2D> ().velocity.magnitude);
		if (time > 2) { //optimisation, makes the physics grid ignore objects that are too far away
			time = 0;
			GameObject player = GameObject.Find("Player");

			GameObject[] gs = GameObject.FindObjectsOfType<GameObject>();
			if (player != null) {
				foreach (GameObject r in gs) {
					//bool isRope = r.GetComponent<JointScript>() != null;
					if (r.GetComponent<DebrisStart>() || r.GetComponent<Loot>() || r.GetComponent<ItemDissolve>()) { //only change active state if it has the debris start or is loot, (ignoring edgebounds, joints, the ship, etc.)
						if (Vector3.Distance(player.transform.position, r.transform.position) > 60) { //everthing outside of a 100 radius will stop
							frozen.Add(r.gameObject);
							r.gameObject.SetActive(false);
							if (r.GetComponent<DebrisStart>()) {
								Transform[] ts = r.GetComponentsInChildren<Transform>();
								foreach (Transform tt in ts) {
									tt.gameObject.SetActive(false);
								}
								//r.SendMessage("Stop");
							}
							//r.Sleep();
						}/* else {
							r.gameObject.SetActive(true);
							//r.WakeUp();
							if (r.GetComponent<DebrisStart>()) {
								Transform[] ts = r.GetComponentsInChildren<Transform>();
								foreach (Transform tt in ts) {
									tt.gameObject.SetActive(true);
								}
								//r.SendMessage("ReStart");
							}
						}*/
					}
					if (r.GetComponent<JointScript>()) {
						if (Vector3.Distance(player.transform.position, r.transform.position) > 60) { //everthing outside of a 100 radius will stop
							r.GetComponent<Rigidbody2D>().Sleep();
						} else {
							r.GetComponent<Rigidbody2D>().WakeUp();
						}
					}
				}
				ArrayList temp = new ArrayList();
				foreach (GameObject r in frozen) {
					if ((player != null &&  r != null) && Vector3.Distance(player.transform.position, r.transform.position) < 60) {
						r.SetActive(true);
						if (r.GetComponent<DebrisStart>()) {
							Transform[] ts = r.GetComponentsInChildren<Transform>();
							foreach (Transform tt in ts) {
								tt.gameObject.SetActive(true);
							}
							//r.SendMessage("ReStart");
						}

					} else {
						temp.Add(r.gameObject);
					}
				}
				frozen = temp;
			}
		}
		//GameObject[] gg = Resources.FindObjectsOfTypeAll<GameObject>();
		//foreach (GameObject g in gg) g.SetActive(true);
		//update area outside
	}


}