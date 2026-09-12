class_name SelfDeletingNode
extends Node3D

@onready var timer:Timer = $Timer;

func OnTimeout() -> void:
	queue_free();
