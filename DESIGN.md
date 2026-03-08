# Documento de diseño

## 1. Concepto general

- **Título:** *pendiente*
- **Género:** Roguelite táctico por turnos
- **Estética:** Medieval fantasía
- **Vista:** Cuadrícula isométrica (2D pixel art)

Cada **run** se compone de **4 encuentros**, siendo el **4º encuentro un boss final** en un escenario con más dificultad.

La progresión de la run se organiza en un **mapa de nodos** (inspirado en *Slay the Spire*, pero reducido): el jugador avanza nodo a nodo y puede **elegir el camino** a seguir hasta llegar al boss final. El boss es **obligatorio y único** (sin elección de ruta en el último tramo).

---

## 2. Composición del equipo

Al iniciar la run, el jugador elige un equipo compuesto por **3 unidades** pudiendo repetir clases de unidades.

### 2.1 Clases 

| Stat | Caballero | Arquero | Mago |
|---|---:|---:|---:|
| **HP máx** | 30 | 25 | 20 |
| **MOV** | 3 | 5 | 4 |
| **AP/turno** | 2 | 2 | 2 |
| **ATK** | 7 | 6 | 2 |
| **MAG** | - | - | 7 |
| **DEF** | 3 | 1 | 1 |
| **ACC** | 80% | 85% | 90% |
| **EVA** | 3% | 7% | 3% |
| **CRIT** | 5% | 5% | 5% |
| **MP máx** | - | - | 4 |
| **Reg MP** | - | - | +1 por turno |
| **Rango básico** | 1 | 5 | 4 |
| **SPEED** | 1 | 3 | 2 |

### 2.2 Habilidades por clase
*pendiente*

---

## 3. Sistema de turnos y economía de acciones

### 3.1 Orden de turno (iniciativa)
Cada unidad tiene un atributo **Speed**.

- El orden de los turnos se determina por **mayor Speed**.
- En caso de empate, el desempate se decide **de forma aleatoria** al inicio, pudiendo saber en todo momento el orden de acciones.

### 3.2 Puntos de acción (AP)
Cada unidad dispone de **2 AP por turno**.

#### Acciones y coste:
- **Moverse:** 1 AP 
  - Distancia máxima: depende del stat de movimiento (*MOV*).
- **Ataque básico:** 1 AP 
- **Habilidades:** Coste dependiente de la habilidad.
- **Descanso / Esperar:** termina el turno sin gastar acciones adicionales (o como acción voluntaria).
- **Objetos:** 0 AP 
  - Restricción: **solo 1 objeto por unidad y turno**.

---

## 4. Estados alterados y buffs

### 4.1 Estados (solo puede haber uno afectando a la vez):

- **Parálisis:** −1 AP durante X turnos.
- **Slow:** −X MOV durante Y turnos.
- **Veneno:** X daño al inicio del turno durante Y turnos, el daño se aumenta cada turno.

### 4.2 Bufos/Debufos:

- **Brave:** +X ataque durante Y turnos.
- **Strength:** +X defensa durante Y turnos.
- **Vulnerable:** defensa a 0 durante X turnos.

---

## 5. Mapa y reglas tácticas

### 5.1 Representación y navegación
- El combate ocurre en una **cuadrícula isométrica**.
- El terreno incluye:
  - **Alturas** (diferencias de nivel).
  - **Coberturas** (muros/obstáculos).

### 5.2 Altura (ventaja táctica)
La altura afecta al daño aunque solo cuando la diferencia es de 1 nivel.

- Si una unidad ataca **desde una altura superior**, obtiene un **bonus de daño**.
- Si ataca **desde una altura inferior**, recibe un **penalizador de daño**.

### 5.3 Línea de visión (LoS) y coberturas
- Para ataques a distancia (**Arquero** y **Magos**), si no existe **línea de visión directa**, el ataque **se bloquea contra el muro/obstáculo**.
- Los muros/obstáculos sirven como cobertura y también como bloqueo de LoS.

### 5.4 Ataque en equipo (cooperación)
Existe una ventaja táctica basada en posicionamiento:

- Si una unidad realiza un ataque físico a un enemigo y el objetivo está **rodeado** por otro u otros aliados, los **compañeros también atacan** (ataque adicional).

---

## 6. Generación procedural de mapas y encuentros

### 6.1 Generación procedural
Cada encuentro genera su mapa de forma **procedural**, en función de:
- El **tipo de encuentro**.
- La **dificultad** del encuentro.
- La **progresión de la run** (cada fase tiende a ser más compleja).

Además:
- La complejidad aumenta conforme avanza la run:
  - más enemigos, layouts más exigentes, más presión táctica.

### 6.2 Tipos de encuentro
- **Básico:** combate estándar.
- **Dificultad aumentada (élite):** combate más difícil con **mejores recompensas**.

---

## 7. Boss final
El último encuentro de la run es un **boss final obligatorio**, con:
- mapa más grande y más complejo
- mayor dificultad

---