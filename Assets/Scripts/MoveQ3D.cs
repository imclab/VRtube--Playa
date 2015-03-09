/** This lives on the Q3Dplayer prefab
 * It's a utility for moving the Q3D program and rescaling it
 * It also saves the position (etc)data whenever it's changed
 * 
 * Because this lives on Q3DPlayer, it only records key movements when the player is activated
 */
//TODO


using UnityEngine;
using System.Collections;
using QuaternionSoft.Q3D.UnityPlayer;
using System.IO;

public class MoveQ3D : MonoBehaviour {

	private bool debug;
	private bool loop;
	public Vector3 del = new Vector3(0.5f,0.5f,0.5f); //How much to move the file
	public float delRot = 10f; //What are these units?
	public float sY = 0.1f;
	private Q3DRenderer rendererQ3D;
	private System.IO.TextWriter textWriter; //This is for saving the file
	private int sign;
	private float factor;
	public int cntrlFactor = 5;

	void Start () {
		rendererQ3D = GetComponentInParent<Q3DRenderer> ();

	}
	//If reading location data from file, move the Q3DPlayer accordingly
	public void MovePlayer(string[] position, string[] rotation, string[] scale, bool moveDebug, bool moveLoop)
	{
		transform.position = new Vector3 (System.Convert.ToSingle (position [0]), System.Convert.ToSingle (position [1]), System.Convert.ToSingle (position [2]));
		transform.rotation = new Quaternion (System.Convert.ToSingle (rotation [0]), System.Convert.ToSingle (rotation [1]), System.Convert.ToSingle (rotation [2]), System.Convert.ToSingle (rotation [3]));
		transform.localScale = new Vector3 (System.Convert.ToSingle (scale [0]), System.Convert.ToSingle (scale [1]), System.Convert.ToSingle (scale [2]));
		debug = moveDebug;
		loop = moveLoop;


	}
	public void WriteToFile()
	{
		//Make the string for where the config data should be stored
		string loc = rendererQ3D.Filename;
		loc = loc.Substring (0, loc.Length - 3);
		loc = string.Concat (loc, "txt");
		//Write it all to file
		textWriter = new System.IO.StreamWriter(loc);
		textWriter.WriteLine("# Welcome to your VRtube Holographic configuration file!");
		textWriter.WriteLine("#Feel free to edit!");
		textWriter.WriteLine("#The Debug setting (if turned on) lets you move the hologram around in real time");
		textWriter.WriteLine("DEBUG=" + debug.ToString());
		//If I could be any animal other than a human, I would be a dog
		textWriter.WriteLine ("POSITION=" + transform.position.x + "," + transform.position.y + "," + transform.position.z);
		textWriter.WriteLine ("ROTATION=" + transform.rotation.x + "," + transform.rotation.y + "," + transform.rotation.z + "," + transform.rotation.w);
		textWriter.WriteLine ("SCALE=" + transform.localScale.x + "," + transform.localScale.y + "," + transform.localScale.z);
		textWriter.WriteLine("LOOP=" + loop.ToString());
		// close the stream
		textWriter.Close();
	}
	// Update is called once per frame
	void Update () {
		if (debug) {
			//This allows Shift to make the command do the reverse of what it usually does
			if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
				factor = -1;
			}
			else{
				factor = 1;
			}
			//This allows CNTRL to make the command a smaller change
			if (Input.GetKey (KeyCode.LeftControl)) {
				factor/=cntrlFactor;
				//print (factor);
			}
			if( Input.GetKey (KeyCode.RightControl)){
				factor/=cntrlFactor;
			}

			//Below lies the ugly mass of code to handle user commands
			if (Input.GetKeyDown (KeyCode.PageUp)) {
				transform.position = new Vector3 (transform.position.x, transform.position.y + del.y * factor, transform.position.z);
				WriteToFile ();
			} else if (Input.GetKeyDown (KeyCode.PageDown)) {
				transform.position = new Vector3 (transform.position.x, transform.position.y - del.y * factor, transform.position.z);
				WriteToFile ();
			} else if (Input.GetKeyDown (KeyCode.U)) {
				transform.localScale += new Vector3 (sY, sY, sY) * factor;
				WriteToFile ();
			} else if (Input.GetKeyDown (KeyCode.I)) {
				transform.localScale -= new Vector3 (sY, sY, sY) * factor;
				WriteToFile ();
			} else if (Input.GetKeyDown (KeyCode.L)) {
				transform.position = new Vector3 (transform.position.x + del.x * factor, transform.position.y, transform.position.z);
				WriteToFile ();
			} else if (Input.GetKeyDown (KeyCode.H)) {
				transform.position = new Vector3 (transform.position.x - del.x * factor, transform.position.y, transform.position.z);
				WriteToFile ();
			} else if (Input.GetKeyDown (KeyCode.K)) {
				transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z + del.z * factor);
				WriteToFile ();
			} else if (Input.GetKeyDown (KeyCode.J)) {
				transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z - del.z * factor);
				WriteToFile ();
			} else if (Input.GetKeyDown (KeyCode.X)) {
				transform.Rotate (delRot * factor, 0, 0);
				WriteToFile ();
			} else if (Input.GetKeyDown (KeyCode.Y)) {
				transform.Rotate (0, delRot * factor, 0);
				WriteToFile ();
			} else if (Input.GetKeyDown (KeyCode.Z)) {
				transform.Rotate (0, 0, delRot * factor);
				WriteToFile ();
			}
		}
		}
}
/* This file is part of VRtube, Playa.

    VRtube, Playa is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    VRtube, Playa is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with VRtube, Playa.  If not, see <http://www.gnu.org/licenses/>.
*/
