# Tactical Loop

**Tactical Loop** es un videojuego roguelike táctico por turnos en 2D y con cuadrícula isométrica, ambientado en un entorno de fantasía medieval. En cada partida, el jugador forma un equipo de tres unidades y avanza a través de un recorrido de cuatro combates organizados en un mapa de nodos, donde deberá tomar decisiones tanto en la ruta seguida como en la estrategia en el combate. Cada batalla plantea una situación táctica distinta hasta llegar a un combate final contra un jefe.

---

## PEC 2

Para esta primera entrega se han implementado las funcionalidades básicas del sistema de combate táctico por turnos:

- Creación de mapas a partir de ficheros `.txt`, donde cada carácter define un elemento de la cuadrícula.
- Definición e implementación de distintos tipos de unidades.
- Sistema de gestión de turnos basado en iniciativa.
- Sistema de puntos de acción (AP).
- Movimiento de unidades en el mapa.
- Acción de ataque básico.
- Sistema de spawn de unidades.
- Selector inicial de unidades.

> **Nota:**  
> Estaba previsto incluir la IA de enemigos y las condiciones de victoria/derrota, pero no se ha llegado a tiempo. Además, muchas funcionalidades utilizan assets temporales. En la siguiente PEC se trabajará en definir el estilo visual definitivo del proyecto.

---

## PEC 3

-  IA de los enemigos.
- Condiciones de victoria/derrota.
- Pantalla de inicio y fin de partida.
- Inventario y objetos.
- Habilidades de las unidades.
- Efectos y estados.
- Mapa de nodos de los encuentros.
- Ataque en equipo.
- Recompensas.

## Estructura actual del juego

Al iniciar el juego, el jugador accede a la pantalla de inicio, donde podrá configurar opciones de audio, video e idioma o podrá iniciar partida.

Al iniciar la partida, el jugador accede a una pantalla de selección de unidades, donde debe escoger tres unidades entre las clases disponibles (permitiendo repetir clases).

Una vez confirmada la selección:
1. Se muestra el mapa de nodos y se pide al jugador escoger entre las siguientes posibilidades de enfrentamientos.
2. Se inicia el mapa correspondiente.
3. Se muestran las zonas de spawn disponibles (en verde).
4. El jugador selecciona la posición inicial de cada unidad.
5. Tras posicionar las tres unidades, comienza la batalla.
6. Si el jugador pierde, se muestra la escena de final del juego permitiendo volver a intentar o salir.
7. Si el jugador gana, si no es el jefe final el que se ha enfrentado se le muestran las recompensas de victoria a escoger y se vuelve al punto 1.
8. Si el jugadro gana y es el jefe final se le muestra la escena de final del juego con mensaje de victoria, permitiendo ver los créditos, volver a jugar o salir del juego.

---

## Interfaz durante la partida

### Panel izquierdo (orden de turnos)

- Muestra el orden de actuación de las unidades.
- Actualmente se visualiza:
  - Tipo de unidad
  - Equipo al que pertenece
  - Indicador de turno actual

---

### Panel derecho (acciones)

Acciones disponibles:
- **Movimiento**
- **Atacar**
- **Habilidades**
- **Objetos**
- **Esperar**

#### Movimiento
- Muestra las posiciones disponibles según el rango de movimiento.
- El jugador selecciona una casilla válida dentro del rango.

#### Atacar
- Muestra las posiciones dentro del rango de ataque.
- Permite seleccionar cualquier objetivo (incluye fuego amigo).
- Si no hay objetivo, el ataque falla pero consume la acción.

#### Habilidades
- Muestra las habilidades disponibles por la unidad y permite seleccionarlas para usarlas.

#### Objetos
- Muestra los objetos disponibles en la partida y permite seleccionar uno para usarlo en el turno.

#### Esperar
- Finaliza el turno de la unidad actual.

---

## Controles

El juego permite el uso tanto de ratón como de teclado.

### Ratón
- Mover cursor: seleccionar elementos  
- Click izquierdo: confirmar acción  
- Click derecho: cancelar acción  

### Teclado

#### Movimiento
- Derecha: `→` o `D`
- Izquierda: `←` o `A`
- Arriba: `↑` o `W`
- Abajo: `↓` o `S`

#### Acciones
- Confirmar: `Enter` o `Espacio`
- Cancelar: `Escape`


## UNIDADES

| Unidad        | HP | MP | MP+ | Mov | AP | Atk | Def | Speed | Range | Acc |   Ev | CritChance | Skills                                                                                 |
| ------------- | -: | -: | --: | --: | -: | --: | --: | ----: | ----: | --: | ---: | ---------: | -------------------------------------------------------------------------------------- |
| Arquero       | 25 |  0 |   0 |   5 |  3 |   6 |   2 |     3 |     1 | 0.8 | 0.08 |       0.05 | Shot: Dmg 10, no fail<br>Shot Poison: Dmg 5, no fail<br>Shot Paralysis: Dmg 5, no fail |
| Maga          | 20 |  5 |   1 |   2 |  3 |   2 |   2 |     2 |     1 | 0.9 | 0.03 |       0.05 | Fire Ball: Dmg 20, no fail                                                             |
| Knight        | 30 |  0 |   0 |   5 |  3 |   7 |   3 |     1 |     1 | 0.7 | 0.03 |       0.05 | Thrust: Dmg 10, no def, no fail                                                        |
| Goblin Basic  | 20 |  0 |   0 |   3 |  3 |   4 |   2 |     1 |     1 | 0.8 | 0.15 |        0.1 | Blow: Dmg 5                                                                            |
| Goblin Lancer | 25 |  0 |   0 |   3 |  3 |   5 |   2 |     1 |     2 | 0.8 | 0.15 |        0.1 | Thrust: Dmg 7, no fail                                                                 |
| Goblin Tank   | 35 |  0 |   0 |   3 |  3 |   3 |   5 |     1 |     1 | 0.8 | 0.15 |        0.1 | Thrust: Dmg 3, no fail                                                                 |
| Boss          | 50 |  5 |   2 |   3 |  3 |   9 |   5 |     2 |     1 |   1 |  0.1 |        0.2 | BlastBurn: Dmg 5, no fail, no def<br>SpectralFire: Dmg 25, no fail                     |
