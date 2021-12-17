const birdElem = document.querySelector( "[data-bird]" )
const BIRD_SPEED = 0.5
const JUMP_DURATION = 150

let timeSinceLastJump = Number.POSITIVE_INFINITY

const setTop = ( top ) => birdElem.style.setProperty( "--bird-top", top )

const getTop = () => parseFloat( getComputedStyle( birdElem ).getPropertyValue( "--bird-top" ) )

const handleJump = ( event ) => {
    if ( event.code === "Space" || event.type == "click") timeSinceLastJump = 0
}

export const setupBird = () => {
    setTop( window.innerHeight / 2 )
    document.removeEventListener( "keydown", handleJump )
    document.addEventListener( "keydown", handleJump )
    document.removeEventListener( "click", handleJump )
    document.addEventListener( "click", handleJump )
}

export const updateBird = ( delta ) => {
    if ( timeSinceLastJump < JUMP_DURATION )
        setTop( getTop() - BIRD_SPEED * delta )
    else
        setTop( getTop() + BIRD_SPEED * delta )

    timeSinceLastJump += delta
}

export const getBirdRect = () => birdElem.getBoundingClientRect()