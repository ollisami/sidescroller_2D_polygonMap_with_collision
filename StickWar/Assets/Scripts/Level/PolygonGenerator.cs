using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PolygonGenerator : MonoBehaviour {
	
	private List<Vector3> meshVertices = new List<Vector3>();
	private List<int> meshTriangles = new List<int>();
	private List<Vector2> newUV = new List<Vector2>();
	
	private List<Vector3> colVertices = new List<Vector3>();
	private List<int> colTriangles = new List<int>();
	private int colCount;
	
	private Mesh mesh;
	private MeshCollider col;
	
	private float tUnit = 0.25f;
	private Vector2 tStone = new Vector2 (1, 0);
	private Vector2 tGrass = new Vector2 (0, 1);
	
	
	private byte[,] blocks;
	private int squareCount;
	public bool update=false;

	private int sizeX;
	private int sizeY;

	private Vector2 posOnMap;
	

	public void createPolygons(int sizeX, int sizeY, Vector2 pos) {
		this.posOnMap = pos;
		this.sizeX = sizeX;
		this.sizeY = sizeY;

		mesh = GetComponent<MeshFilter> ().mesh;
		col = GetComponent<MeshCollider> ();

		GenTerrain();
		BuildMesh();
		UpdateMesh();
	}
	
	void Update(){
		if(update){
			BuildMesh();
			UpdateMesh();
			update=false;
		}
	}
	
	
	private int NoiseInt (int x, int y, float scale, float mag, float exp){
		x += (int)transform.position.x;
		return (int) (Mathf.Pow ((Mathf.PerlinNoise(x/scale,y/scale)*mag),(exp) ));		
	}
	
	private void GenTerrain(){
		blocks=new byte[sizeX,sizeY];  // leveys / korkeus
		
		for(int px=0;px<blocks.GetLength(0);px++){
			int stone= NoiseInt(px,0,80,15,1);
			stone+= NoiseInt(px,0,50,30,1);
			stone+= NoiseInt(px,0,10,10,1);
			stone+=75;
			
			
			int dirt = NoiseInt(px,0,100,35,1);
			dirt+= NoiseInt(px,100, 50,30,1);
			dirt+=75;
			
			
			for(int py=0;py<blocks.GetLength(1);py++){
				if(py<stone){
					blocks[px, py]=1;
					
					if(NoiseInt(px,py,12,16,1)>10){		//dirt spots
						blocks[px,py]=2;
						
					}
					
					if(NoiseInt(px,py*2,16,14,1)>10){	//Caves
						blocks[px,py]=0;
						
					}
					
				} else if(py<dirt) {
					blocks[px,py]=2;
				}
				
				
			}
		}
	}
	
	private void BuildMesh(){
		for(int px=0;px<blocks.GetLength(0);px++){
			for(int py=0;py<blocks.GetLength(1);py++){
				
				if(blocks[px,py]!=0){
					GenCollider(px,py);

					if(blocks[px,py]==1){
						GenSquare(px,py,tStone);
					} else if(blocks[px,py]==2){
						GenSquare(px,py,tGrass);
					}
				}
			}
		}
	}
	
	private byte Block (int x, int y){
		
		if(x==-1 || x==blocks.GetLength(0) || y==-1 || y==blocks.GetLength(1)){
			return (byte)1;
		}
		
		return blocks[x,y];
	}


	private void GenCollider(int x, int y){
			
		//Top
		if(Block(x,y+1)==0){
		colVertices.Add( new Vector3 (x		,  y		, 1));
		colVertices.Add( new Vector3 (x + 1	,  y		, 1));
		colVertices.Add( new Vector3 (x + 1	,  y	, 0	));
		colVertices.Add( new Vector3 (x		,  y		, 0	));
		
		ColliderTriangles();
		
		colCount++;
		}
		
		//bot
		if(Block(x,y-1)==0){
		colVertices.Add( new Vector3 (x		,  y -1	, 0));
		colVertices.Add( new Vector3 (x + 1	,  y	-1	, 0));
		colVertices.Add( new Vector3 (x + 1	,  y	-1	, 1	));
		colVertices.Add( new Vector3 (x		,  y	-1	, 1	));
		
		ColliderTriangles();
		colCount++;
		}
		
		//left
		if(Block(x-1,y)==0){
		colVertices.Add( new Vector3 (x		,  y	-1	, 1));
		colVertices.Add( new Vector3 (x		,  y		, 1));
		colVertices.Add( new Vector3 (x 	,  y		, 0	));
		colVertices.Add( new Vector3 (x		,  y	-1	, 0	));
		
		ColliderTriangles();
		
		colCount++;
		}
		
		//right
		if(Block(x+1,y)==0){
		colVertices.Add( new Vector3 (x	+1	,  y		, 1));
		colVertices.Add( new Vector3 (x	+1	,  y	-1	, 1));
		colVertices.Add( new Vector3 (x +1	,  y	-1	, 0	));
		colVertices.Add( new Vector3 (x	+1	,  y		, 0	));
		
		ColliderTriangles();
		
		colCount++;
		}
		
	}
	
	private void ColliderTriangles(){
		colTriangles.Add(colCount*4);
		colTriangles.Add((colCount*4)+1);
		colTriangles.Add((colCount*4)+3);
		colTriangles.Add((colCount*4)+1);
		colTriangles.Add((colCount*4)+2);
		colTriangles.Add((colCount*4)+3);
	}

	private void MeshTriangles(){
		meshTriangles.Add(squareCount*4);
		meshTriangles.Add((squareCount*4)+1);
		meshTriangles.Add((squareCount*4)+3);
		meshTriangles.Add((squareCount*4)+1);
		meshTriangles.Add((squareCount*4)+2);
		meshTriangles.Add((squareCount*4)+3);
	}
	
	
	private void GenSquare(int x, int y, Vector2 texture){

		// Front 
		meshVertices.Add( new Vector3 (x		,  y		, 0	));
		meshVertices.Add( new Vector3 (x + 1	,  y		, 0	));
		meshVertices.Add( new Vector3 (x + 1	,  y-1	, 0	));
		meshVertices.Add( new Vector3 (x		,  y-1	, 0	));
		
		MeshTriangles ();	
		newUV.Add(new Vector2 (tUnit * texture.x, tUnit * texture.y + tUnit));
		newUV.Add(new Vector2 (tUnit * texture.x + tUnit, tUnit * texture.y + tUnit));
		newUV.Add(new Vector2 (tUnit * texture.x + tUnit, tUnit * texture.y));
		newUV.Add(new Vector2 (tUnit * texture.x, tUnit * texture.y));		
		squareCount++;

		//Top
		if(Block(x,y+1)==0){
			meshVertices.Add( new Vector3 (x		,  y		, 1));
			meshVertices.Add( new Vector3 (x + 1	,  y		, 1));
			meshVertices.Add( new Vector3 (x + 1	,  y	, 0	));
			meshVertices.Add( new Vector3 (x		,  y		, 0	));

			MeshTriangles ();	
			newUV.Add(new Vector2 (tUnit * texture.x, tUnit * texture.y + tUnit));
			newUV.Add(new Vector2 (tUnit * texture.x + tUnit, tUnit * texture.y + tUnit));
			newUV.Add(new Vector2 (tUnit * texture.x + tUnit, tUnit * texture.y));
			newUV.Add(new Vector2 (tUnit * texture.x, tUnit * texture.y));		
			squareCount++;
		}

		//bot
		if(Block(x,y-1)==0){
			meshVertices.Add( new Vector3 (x		,  y -1	, 0));
			meshVertices.Add( new Vector3 (x + 1	,  y	-1	, 0));
			meshVertices.Add( new Vector3 (x + 1	,  y	-1	, 1	));
			meshVertices.Add( new Vector3 (x		,  y	-1	, 1	));

			MeshTriangles ();	
			newUV.Add(new Vector2 (tUnit * texture.x, tUnit * texture.y + tUnit));
			newUV.Add(new Vector2 (tUnit * texture.x + tUnit, tUnit * texture.y + tUnit));
			newUV.Add(new Vector2 (tUnit * texture.x + tUnit, tUnit * texture.y));
			newUV.Add(new Vector2 (tUnit * texture.x, tUnit * texture.y));		
			squareCount++;
		}

		//left
		if(Block(x-1,y)==0){
			meshVertices.Add( new Vector3 (x		,  y	-1	, 1));
			meshVertices.Add( new Vector3 (x		,  y		, 1));
			meshVertices.Add( new Vector3 (x 	,  y		, 0	));
			meshVertices.Add( new Vector3 (x		,  y	-1	, 0	));

			MeshTriangles ();	
			newUV.Add(new Vector2 (tUnit * texture.x, tUnit * texture.y + tUnit));
			newUV.Add(new Vector2 (tUnit * texture.x + tUnit, tUnit * texture.y + tUnit));
			newUV.Add(new Vector2 (tUnit * texture.x + tUnit, tUnit * texture.y));
			newUV.Add(new Vector2 (tUnit * texture.x, tUnit * texture.y));		
			squareCount++;
		}

		//right
		if(Block(x+1,y)==0){
			meshVertices.Add( new Vector3 (x	+1	,  y		, 1));
			meshVertices.Add( new Vector3 (x	+1	,  y	-1	, 1));
			meshVertices.Add( new Vector3 (x +1	,  y	-1	, 0	));
			meshVertices.Add( new Vector3 (x	+1	,  y		, 0	));

			MeshTriangles ();	
			newUV.Add(new Vector2 (tUnit * texture.x, tUnit * texture.y + tUnit));
			newUV.Add(new Vector2 (tUnit * texture.x + tUnit, tUnit * texture.y + tUnit));
			newUV.Add(new Vector2 (tUnit * texture.x + tUnit, tUnit * texture.y));
			newUV.Add(new Vector2 (tUnit * texture.x, tUnit * texture.y));		
			squareCount++;
		}
		
	}
	
	private void UpdateMesh () {
		mesh.Clear ();
		mesh.vertices = meshVertices.ToArray();
		mesh.triangles = meshTriangles.ToArray();
		mesh.uv = newUV.ToArray();
		mesh.Optimize ();
		mesh.RecalculateNormals ();
		
		meshVertices.Clear();
		meshTriangles.Clear();
		newUV.Clear();
		squareCount=0;
		
		Mesh newMesh = new Mesh();
		newMesh.vertices = colVertices.ToArray();
		newMesh.triangles = colTriangles.ToArray();
		col.sharedMesh= newMesh;
		
		colVertices.Clear();
		colTriangles.Clear();
		colCount=0;
	}

	public byte[,] getBlocks() {
		return this.blocks;
	}

	public void setBlock(int x, int y, byte value) {
		if (x <= this.blocks.GetLength (0) && y <= this.blocks.GetLength (1)) {
			this.blocks [x, y] = value;
			update = true;
		}
	}

	public Vector2 getPos() {
		return this.posOnMap;
	}
}
