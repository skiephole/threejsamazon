class gNode extends THREE.Group
{
	constructor()
	{
		super();

		this._loadState = LoadStates.NOT_LOADING;

		this.init();
	}

	get loadState()
	{
		return this._loadState;
	}

	init()
	{
		function addSpotLight(object, color, x, y, z, intensity, targetx, targety, targetz)
		{
			//spotlight values in order= color, intensity, distance, angle, penumbra, decay
			var spotLight = new THREE.SpotLight(color, intensity, 100, 0.5, 2, 1);
			spotLight.position.set(x, y, z);
			spotLight.castShadow = true;
			object.add(spotLight);
			object.add(spotLight.target);
			spotLight.target.position.set(targetx, targety, targetz)
		}

		function addPointLight(object, color, x, y, z, intensity, distance)
		{
			var pointLight = new THREE.PointLight(color, intensity, distance);
			pointLight.position.set(x, y, z);
			object.add(pointLight);
		}

		if (this._loadState != LoadStates.NOT_LOADING) return;

		this._loadState = LoadStates.LOADING;

		var nodeGeometry = new THREE.PlaneGeometry(2, 2, 2);
		var nodeMaterial = new THREE.MeshLambertMaterial({ color: 0x000000, side: THREE.DoubleSide });
		var node = new THREE.Mesh(nodeGeometry, nodeMaterial);

		addPointLight(node, 0xd80808, 0, 0, -1, 2, 4);

		this.add(node);
	}
}
