
# Projet Synthèse - Version de démarrage

[![Build Status](https://gitlab.com/csf-game-dev/projet-synthese-start/badges/master/build.svg)](https://gitlab.com/csf-game-dev/projet-synthese-start/commits/master) [![Tests Report](https://img.shields.io/badge/test-report-brightgreen.svg)](https://csf-game-dev.gitlab.io/projet-synthese-start/testreport/Index.html) [![Documentation](https://img.shields.io/badge/doc-report-brightgreen.svg)](https://csf-game-dev.gitlab.io/projet-synthese-start/doc/html/index.html)

Ce dépôt contient un projet Unity de base. Ce dernier possède une grande quantité d'outils forts utiles et est aussi préconfiguré 
pour effectuer de l'intégration continue.

## Démarrage rapide

Ces instructions vous permettront de vous créer une copie fonctionnelle du projet sur votre machine à des fins de développement 
et de test.

### Prérequis

Pour utiliser ce projet de démarrage, vous aurez besoin de :

* Unity
* Visual Studio

Ce projet utilise [GitLab-CI](https://docs.gitlab.com/ce/ci/README.html) afin de compiler, tester et déployer le projet à chaque mise 
à jour de la branche ```master```. Vous aurez donc besoin d'une **Build machine** sous Windows avec 
[Gitlab-Runner](https://docs.gitlab.com/runner/). [Installez-le](https://docs.gitlab.com/runner/install/) et assurez-vous de 
configurer la variable d'environnement ```PATH``` pour y inclure le chemin vers le dossier contenant l'exécutable de Unity.

Par exemple :
```
C:\Program Files\7-Zip;C:\Program Files\GitLab-Runner;C:\Program Files\Unity\Editor
```

## Technologies utilisées

* [Unity](https://unity3d.com/) - Le moteur de jeu utilisé.
* [NUnit](https://www.nunit.org/) - Framework de tests unitaires.
* [NSubstitute](http://nsubstitute.github.io/) - Framework de Mocks. La version incluse a été spécifiquement conçue pour Unity. Extraite des [UnityTestTools](https://www.assetstore.unity3d.com/en/#!/content/13802).
* [Inno Setup](http://www.jrsoftware.org/isinfo.php) - L'outil de création de fichier d'installation.

## Auteurs

* Benjamin Lemelin - Programmation de l'outil d'injection de dépendances

## License

Ce projet est autorisé sous licence MIT - voir le fichier [LICENSE.md] (LICENSE.md) pour plus de détails

## Remerciements

* [Jawnnypoo](https://github.com/Commit451/skyhook) - Pour son site [SkyHook](https://skyhook.glitch.me/), qui permet de convertir un WebHook de GitLab vers un WebHook pour Discord. C'est super pratique pour recevoir des messages sur le status actuel du projet.