# Tactical Loop

**Tactical Loop** es un videojuego roguelike táctico por turnos en 2D y con cuadrícula isométrica, ambientado en un entorno de fantasía medieval. En cada partida, el jugador forma un equipo de tres unidades y avanza a través de un recorrido de cuatro combates organizados en un mapa de nodos, donde deberá tomar decisiones tanto en la ruta seguida como en la estrategia en el combate. Cada batalla plantea una situación táctica distinta hasta llegar a un combate final contra un jefe.

---

## PEC 2

### Implementaciones

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

## Estructura actual del juego

Al iniciar el juego, el jugador accede a una pantalla de selección de unidades, donde debe escoger tres unidades entre las clases disponibles (permitiendo repetir clases).

Una vez confirmada la selección:

1. Se inicia una partida en un mapa básico.
2. Se muestran las zonas de spawn disponibles (en verde).
3. El jugador selecciona la posición inicial de cada unidad.
4. Tras posicionar las tres unidades, comienza la batalla.

---

## Interfaz durante la partida

### Panel izquierdo (orden de turnos)

- Muestra el orden de actuación de las unidades.
- Actualmente se visualiza:
  - Tipo de unidad
  - Equipo al que pertenece
  - Indicador de turno actual

**A futuro:**
- Uso de iconos/imágenes en lugar de texto.
- Diferenciación visual de equipos (colores o bordes).
- Resaltado de la unidad activa.

---

### Panel derecho (acciones)

Acciones disponibles:
- **Movimiento**
- **Atacar**
- **Esperar**

#### Movimiento
- Muestra las posiciones disponibles según el rango de movimiento.
- El jugador selecciona una casilla válida dentro del rango.

#### Atacar
- Muestra las posiciones dentro del rango de ataque.
- Permite seleccionar cualquier objetivo (incluye fuego amigo).
- Si no hay objetivo, el ataque falla pero consume la acción.

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