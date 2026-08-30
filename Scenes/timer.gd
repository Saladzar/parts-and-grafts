extends Timer


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass

@onready var audio_player = $AudioStreamPlayer

# This function runs automatically when the Timer finishes counting down
func _on_timer_timeout():
	audio_player.play()
