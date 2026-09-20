extends Label

func _ready():
	var powerComponent = get_node("../../Player/PowerComponent")
	powerComponent.powerChanged.connect(_on_power_changed)
	self.text = "0";


func _on_power_changed(powerLevel):
	self.text = str(powerLevel)
