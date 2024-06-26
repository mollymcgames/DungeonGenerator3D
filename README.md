# DungeonGenerator3D
 Project to generate dungeons in Unity 


Instructions:

1. Run `pip install Flask` where you want to make your Flask app.
2. Make a Python file called `app.py` in the same folder where `dungeons_dataset.npy` and `rooms_dataset.npy.npy` is.

The contents of `app.py` is:

```
from flask import Flask, jsonify
import numpy as np

app = Flask(__name__)

@app.route('/', methods=['GET'])
def hello_world():
    return 'Hello World!'

@app.route('/get_dungeon_data', methods=['GET'])
def get_dungeon_data():
    # Load dungeon data
    data = np.load('dungeons_dataset.npy')

    # Select the first dungeon
    selected_dungeon = data[0]

    # Convert the selected dungeon from NumPy array to list
    dungeon_list = selected_dungeon.tolist()

    return jsonify(dungeon_list)

@app.route('/get_room_data', methods=['GET'])
def get_room_data():
    # Load roomd data
    room_data = np.load('rooms_dataset.npy')

    # Select the first room
    selected_room = room_data[0]

    # Convert the selected room from NumPy array to list
    room_list = selected_room.tolist()

    return jsonify(room_list)

if __name__ == '__main__':
    app.run(debug=True)
```

Run the Flask app:

```
python app.py
```
