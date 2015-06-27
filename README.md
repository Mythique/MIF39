# MIF39

Pour lancer l'application :
* Compiler le serveur
<code>cd BASE_C++</code>
<code>mkdir build</code>
<code>cd build</code>
<code>cmake ..</code>
<code>make</code>
* Lancer le serveur se trouvant dans <code>BASE_C++/build/bin/</code> avec la commande <code>./ServerInTheMiddle _Monde.bin_</code>
* Lancer le client Unity en chargeant la scène <code>Unity/Assets/testLoad.unity</code>
* Associer les scripts <code>StartScript</code> et <code>MoveCamera</code> à <code>Main Camera</code>.
* Modifier l'adresse IP du serveur (variable associée à la caméra)
* Lancer la scène !


Pour lancer le test IA :
* Lancer le client Unity en chargeant la scène <code>TestIA/Assets/Scenes/base</code>
* Changer la variable <code>MarshMallowDLLPath</code> de la classe <code>SimpleMarshmallow</code> dans le script <code>SimpleMarshmallow.cs</code> pour être celui de votre .DLL (présent dans <code>MarshmallowDLL\MarshmallowDLL\bin\Debug</code>
* Lancer la scène

La lecture de la DLL peut être remplacé par une méthode de téléchargement (disponible dans) <code>WWWAssemblyLoader.cs</code>
