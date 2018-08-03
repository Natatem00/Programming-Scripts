// Fill out your copyright notice in the Description page of Project Settings.

#include "Grabber.h"
#include "Engine/World.h"
#include "GameFramework/Actor.h"
#include "DrawDebugHelpers.h"

#define OUT

// Sets default values for this component's properties
UGrabber::UGrabber(){

	pPhysicsComponent = nullptr;
	pInputComponent = nullptr;

	PrimaryComponentTick.bCanEverTick = true;
}

// Called when the game starts
void UGrabber::BeginPlay()
{
	Super::BeginPlay();

	//Find components
	FindPhysicComponent();
	FindInputComponent();
}

//"Find components" functions
void UGrabber::FindPhysicComponent()
{
	pPhysicsComponent = GetOwner()->FindComponentByClass<UPhysicsHandleComponent>();
	if (!pPhysicsComponent)
	{
		UE_LOG(LogTemp, Error, TEXT("ERROR! Can't find \"UPhysicsHandleComponent\" in %s"), *(GetOwner()->GetName()))
	}
}
void UGrabber::FindInputComponent()
{
	pInputComponent = GetOwner()->FindComponentByClass<UInputComponent>();
	if (!pInputComponent)
	{
		UE_LOG(LogTemp, Error, TEXT("ERROR! Can't find \"UInputComponent\" in %s"), *(GetOwner()->GetName()))
	}
	else if (pInputComponent)
	{
		///Bind actions on input
		pInputComponent->BindAction("Grab", IE_Pressed, this, &UGrabber::Grab);
		pInputComponent->BindAction("Grab", IE_Released, this, &UGrabber::Release);
	}
}

//Grab and release functions
void UGrabber::Grab()
{
	if (!pPhysicsComponent) 
	{ 
		UE_LOG(LogTemp, Error, TEXT("Can't find \"PhysicsHandleComponent\" in %s"), *(GetOwner()->GetName()))
		return; 
	}
	///Find Hit object
	FHitResult hit = HitObject();
	auto Component = hit.GetComponent();
	///Grab Hit component
	if(hit.GetActor())
		pPhysicsComponent->GrabComponent(Component, NAME_None, hit.GetActor()->GetTransform().GetLocation(), true);
}
void UGrabber::Release()
{
	if (!pPhysicsComponent) 
	{ 
		UE_LOG(LogTemp, Error, TEXT("Can't find \"PhysicsHandleComponent\" in %s"), *(GetOwner()->GetName()))
		return; 
	}
	pPhysicsComponent->ReleaseComponent();
}

/// Called every frame
void UGrabber::TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction)
{
	Super::TickComponent(DeltaTime, TickType, ThisTickFunction);

	if (!pPhysicsComponent) 
	{ 
		UE_LOG(LogTemp, Error, TEXT("Can't find \"PhysicsHandleComponent\" in %s"), *(GetOwner()->GetName())) 
		return; 
	}
	if (pPhysicsComponent->GrabbedComponent) 
	{
		pPhysicsComponent->SetTargetLocation(SetTrackEnd());
	}
}
//Hited object by the ray
FHitResult UGrabber::HitObject()
{
	FHitResult hit;
	///Create Ray-Track
	GetWorld()->LineTraceSingleByObjectType(
		OUT hit,
		SetTrackBegin(),
		SetTrackEnd(),
		FCollisionObjectQueryParams(ECollisionChannel::ECC_PhysicsBody), ///query obj type
		FCollisionQueryParams(FName(TEXT("")), false, GetOwner()) ///(InTraceTag, ComplexTrace, ignoreActor)
	);
	return hit;
}

FVector UGrabber::SetTrackEnd()
{
	///get pawn from FirstPlayerController
	AActor *pPlayer = GetWorld()->GetFirstPlayerController()->GetPawn();

	FVector PlayerPosition = pPlayer->GetTransform().GetLocation();
	FRotator PlayerRotation = pPlayer->GetActorRotation();
	
	///Find *pPlayer ViewPoint
	GetWorld()->GetFirstPlayerController()->GetPlayerViewPoint(OUT PlayerPosition,
		OUT PlayerRotation);

	return PlayerPosition + PlayerRotation.Vector() * TrackRange;
}

FVector UGrabber::SetTrackBegin()
{	///get pawn from FirstPlayerController
	AActor *pPlayer = GetWorld()->GetFirstPlayerController()->GetPawn();

	FVector PlayerPosition = pPlayer->GetTransform().GetLocation();
	FRotator PlayerRotation = pPlayer->GetActorRotation();
	
	///Find *pPlayer ViewPoint
	GetWorld()->GetFirstPlayerController()->GetPlayerViewPoint(OUT PlayerPosition,
		OUT PlayerRotation);

	return PlayerPosition;
}

