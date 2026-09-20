class_name SelfDeletingNode
extends Node3D

@onready var timer:Timer = $Timer;

func _ready():
	timer.timeout.connect(OnTimeout)

func OnTimeout() -> void:
	queue_free();
