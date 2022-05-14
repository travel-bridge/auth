.PHONY: all

REGISTRY?=local
BUILD_NUMBER?=latest
REVISION?=revision-not-specified
TEST_NETWORK?=host
TEST_ENVIRONMENT?=docker

build:
	@echo build services:
	docker build . --rm  --tag ${REGISTRY}/auth_services:$(BUILD_NUMBER) --target services --build-arg revision='${REVISION}'

	@echo build migrator:
	docker build . --rm  --tag ${REGISTRY}/auth_migrator:$(BUILD_NUMBER) --target migrator --build-arg revision='${REVISION}'

	@echo build tests:
		docker build . --rm  --tag ${REGISTRY}/auth_tests:$(BUILD_NUMBER) --target tests

test:
	@echo run tests:
	docker run --rm --network='${TEST_NETWORK}' -e ASPNETCORE_ENVIRONMENT=${TEST_ENVIRONMENT} ${REGISTRY}/auth_tests:$(BUILD_NUMBER)