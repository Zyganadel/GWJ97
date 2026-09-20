extends SelfDeletingNode

func _ready():
	timer.timeout.connect(OnTimeout)
	timer.timeout.connect(get_parent().onAttackTimout)
