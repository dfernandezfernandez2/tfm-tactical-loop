# PLANIFICACIÓN DE TAREAS

## **PEC 2 Estado del arte y primera versión del proyecto - 12/04**

El objetivo principal en esta entrega sería tener un nivel generado a mano funcional. Es decir, implementar las mecánicas dentro del nivel y permitir tener una primera versión de batalla.

    - Escena de juego
        - Crear un fichero .txt que defina un mapa estático, cada caracter es un elemento de la escena del mapa.
        - Conversor del .txt a objeto que defina los elementos del mapa.
        - Render del objeto que define la escena a la escena real de unity.
        - Assets del suelo (tener diferentes variantes) y muros.

    - Unidades
        - Definir una interfaz unidad.
        - Definir una clase stats de unidad con las diferentes stats 
            HP MAX, MP MAX, MOV, AP MAX, ATK, MAG, DEF, ACC, EVA, CRIT, REG MP, RANGE, SPEED
        - Crear una clase por clase de unidad (MAGO, CABALLERO, ARQUERO) que implemente la interfaz unidad y tenga un atributo stat inicializando con el valor definido en el diseño de cada uno.
        - Assets de cada clase de unidad.
        - Animación IDLE.

    - Sistema de turnos y AP (acción por turno)
        - Crear clase que se encarge de gestionar los turnos de una batalla manteniendo una cola de "iniciativa" de las unidades, de forma que de inicio obtenga todas las unidades de la batalla y a partir de su stat SPEED cree la cola.
        - Gestionar las acciones por un turno, si a la unidad no le queda AP en este turno solo se le permite esperar o usar objeto.
        - UI orden de los turnos. Crear al lateral un conjunto de iconos que muestren el orden de las unidades hasta las siguientes N acciones siguientes para conocer en todo momento el orden de turnos.
        - UI de las acciones de la unidad en el turno.

    - Movimiento
        - Path finding: Según el stat MOV de la unidad del turno calculas tiles accesibles y rutas.
        - Mover la unidad: consumir AP, animación de movimiento y actualizar la posición.
        - Highlight del movimiento: Al seleccionar la acción de movimiento mostrar los tiles accesibles por la unidad para que visualmente el jugador sepa donde puede acceder.

    - Ataque básico (melee y rango) - con hit, miss y dmg
        - Al seleccionar la opción de ataque básico remarcar los posibles objetivos al alcance.
        - Permitir la selección del objetivo del ataque por el jugador.
        - Calcular al atacar si hace hit o miss
        - Calcular daño de ataque si hace hit
        - Incluir animación de ataque básico.
        - Incluir animación de recibir daño y de esquivar.
    
    - Sistema de maná.
        - En cada turno aumentar el maná si la unidad tiene regeneración de maná.

    - Spawns y selector de tropas de incio
        - En el mapa definir zonas de spawn enemigo y del jugador.
        - Al inicio de una batalla el jugador podrá seleccionar donde posicionar sus unidades dentro de su zona de spawn.
        - Spawns enemigos aletórios en la zona de spawn enemiga válida.
        - Al inicio de la run crear un selector de tropas (3 unidades).

    - IA enemigos
        Por prioridades:
            - Si puede matar -> ataca para matar.
            - Si está débil se aleja.
            - Si puede atacar -> ataca.
            - Si no, se mueve hacía el objetivo más cercano.
    
    - Condiciones de victoria/derrota.
        - Controlador de batalla, tener control de los equipos que están en el enfrentamiento y si algún equipo se queda sin unidad vidad fin del juego.
        - En caso de que el equipo ganador sea el jugador avanzaría al selector (en este punto no está implementado) para ello inicialmente solamente saldría un mensaje de ganar.
        - En caso que el equipo ganador no sea el jugador se iniciaría la escena de final del juego.

## **PEC 3 Implementación de versión jugable - 03/05**

El objetivo de esta PEC sería tener una versión jugable completa aún sin tener la parte de generación procedural de mapas y parte del gameplay de gestionar las alturas y el LoS.

    - Pantalla de inicio.
    - Inventario
        - Añadir la opción de objetos en la barra de acciones de las unidades.
        - Almacenar los objetos que tiene el jugador durante la run y que le permitan utilizarlas en la acción.
    - Menús de pausa.
    - Fin del juego.
    - Habilidades.
        - Crear las diferentes habilidades de las unidades.
        - Animaciones de las habilidades de las unidades.
        - Incluir la opción de habilidad en la barra de acciones.
    - Sistema de efectos y estados.
        - Contemplar los efectos y estados aplicados sobre una unidad.
        - Modificar el asset del jugador cuando se le ha aplicado un estado.
    - Mapa
        - Generador de mapa por nodos al inicio de la run.
        - Navegación por el mapa de nodos, permitir al jugador seleccionar la siguiente batalla entre una y otra permitiendo escoger entre los caminos que tiene disponibles.
        - Crear 2 mapas adicionales uno con mayor dificultad.
        - Crear 1 mapa adicional del boss final.
    - Ataque en equipo
        - Si al atacar a melee el enemigo tiene otros atacantes al rededor se activa el ataque de los demás.
    - Recompensas entre batallas.

## **PEC 4 Memoria y productos finales -  31/05**

    - Mapas
        - Generador de mapas procedural.
        - Validación del mapa generado para comprobar que es jugable.
    - Guardado en mitad de la run. Poder retomar la run dejada a medias guardando el estado.
    - Gestión alturas.
    - Gestión LoS.