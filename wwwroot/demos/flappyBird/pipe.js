const HOLE_HEIGHT = 250
const PIPE_WIDTH = 138
const PIPE_INTERVAL = 1500
const PIPE_SPEED = 0.5
let pipes = []
let timeSinceLastPipe
let passedPipeCount

const createPipe = () => {
    const pipeElem = document.createElement( "div" )
    const topElem = createPipeSegment( "top" )
    const bottomElem = createPipeSegment( "bottom" )
    pipeElem.append( topElem )
    pipeElem.append( bottomElem )
    pipeElem.classList.add( "pipe" )
    pipeElem.style.setProperty(
        "--hole-top",
        randomNumberBetween(
            HOLE_HEIGHT * 1.5,
            window.innerHeight - HOLE_HEIGHT * 0.5
        )
    )
    const pipe = {
        get left () {
            return parseFloat(
                getComputedStyle( pipeElem ).getPropertyValue( "--pipe-left" )
            )
        },
        set left ( value ) {
            pipeElem.style.setProperty( "--pipe-left", value )
        },
        remove: () => {
            pipes = pipes.filter( p => p !== pipe )
            pipeElem.remove()
        },
        rects: () => [
            topElem.getBoundingClientRect(),
            bottomElem.getBoundingClientRect(),
        ]
    }
    pipe.left = window.innerWidth
    document.body.append( pipeElem )
    pipes.push( pipe )
}

const createPipeSegment = ( position ) => {
    const segment = document.createElement( "div" )
    segment.classList.add( "segment", position )
    return segment
}

const randomNumberBetween = ( min, max ) => Math.floor( Math.random() * ( max - min + 1 ) + min )

export const setupPipes = () => {
    document.documentElement.style.setProperty( "--pipe-width", PIPE_WIDTH )
    document.documentElement.style.setProperty( "--hole-height", HOLE_HEIGHT )
    pipes.forEach( pipe => pipe.remove() )
    timeSinceLastPipe = PIPE_INTERVAL
    passedPipeCount = 0
}

export const updatePipes = ( delta ) => {
    timeSinceLastPipe += delta

    if ( timeSinceLastPipe > PIPE_INTERVAL ) {
        timeSinceLastPipe -= PIPE_INTERVAL
        createPipe()
    }

    pipes.forEach( pipe => {
        if ( pipe.left + PIPE_WIDTH < 0 ) {
            passedPipeCount++
            return pipe.remove()
        }
        pipe.left = pipe.left - delta * PIPE_SPEED
    } )
}

export const getPassedPipesCount = () => passedPipeCount

export const getPipeRects = () => pipes.flatMap( pipe => pipe.rects() )