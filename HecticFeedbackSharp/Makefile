LIB_NAME			:= libSwiftHaptics.dylib
SWIFT_CODE_PATH		:= HecticFeedback/src/swift

all:
	@echo "Compiling Swift code..."
	@cd $(SWIFT_CODE_PATH) && make
	@echo "Copying compiled Swift code in .csproj location..."
	@cp $(SWIFT_CODE_PATH)/$(LIB_NAME) ./

clean:
	@echo "Removing compiled Swift code in .csproj location..."
	@rm -rf ./$(LIB_NAME)
