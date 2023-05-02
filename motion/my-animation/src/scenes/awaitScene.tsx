import { Circle, Img, Rect } from '@motion-canvas/2d/lib/components'
import { makeScene2D } from '@motion-canvas/2d/lib/scenes'
import { all, waitFor } from '@motion-canvas/core/lib/flow'
import { Reference, createRef, useLogger } from '@motion-canvas/core/lib/utils'

import cogSvg from '../images/gear-svgrepo-com.svg'
import workSvg from '../images/work-alt-svgrepo-com.svg'
import {
  easeInBack,
  easeInBounce,
  easeInCirc,
  easeInCubic,
  easeInElastic,
  easeInOutCubic,
  easeOutBack,
  easeOutCubic,
  easeOutQuad,
  linear,
  map,
  tween,
} from '@motion-canvas/core/lib/tweening'
import { PossibleVector2, Vector2 } from '@motion-canvas/core/lib/types'
import { vector2Signal } from '@motion-canvas/2d/lib/decorators'

export default makeScene2D(function* (view) {
  const logger = useLogger()
  const width = 800
  const height = 250
  const cogHeight = 200
  const cogWidth = 200

  const workSize = { x: 200, y: 200 }

  const allCogs = [createRef<Img>(), createRef<Img>(), createRef<Img>(), createRef<Img>()]
  const [cogOne, cogTwo, cogThree, cogFour] = allCogs
  const allWork = [
    createRef<Img>(),
    createRef<Img>(),
    createRef<Img>(),
    createRef<Img>(),
    createRef<Img>(),
    createRef<Img>(),
  ]
  const workContainer = createRef<Rect>()

  const createWorkImg = (ref: Reference<Img>) => <Img ref={ref} src={workSvg} size={workSize} position={[-600, 400]} />
  const [threadContainerOne, threadContainerTwo, threadContainerThree, threadContainerFour] = [
    createRef<Rect>(),
    createRef<Rect>(),
    createRef<Rect>(),
    createRef<Rect>(),
  ]
  view.add(
    <Rect layout width={'100%'} height={'100%'} justifyContent={'end'}>
      <Rect layout height={'100%'} width={'50%'}>
        <Rect ref={workContainer} layout direction={'column'} justifyContent={'end'}></Rect>
      </Rect>
      <Rect
        layout
        direction={'column'}
        height={'100%'}
        justifyContent={'space-around'}
        paddingRight={40}
        paddingTop={40}
        paddingBottom={40}
      >
        <Rect
          stroke={'black'}
          lineWidth={10}
          width={width}
          height={height}
          layout
          justifyContent={'end'}
          alignItems={'center'}
          ref={threadContainerOne}
        >
          <Img ref={cogOne} src={cogSvg} size={workSize} marginRight={40} />
        </Rect>
        <Rect
          stroke={'black'}
          lineWidth={10}
          width={width}
          height={height}
          layout
          justifyContent={'end'}
          alignItems={'center'}
          ref={threadContainerTwo}
        >
          <Img ref={cogTwo} src={cogSvg} size={workSize} marginRight={40} />
        </Rect>
        <Rect
          stroke={'black'}
          lineWidth={10}
          width={width}
          height={height}
          layout
          justifyContent={'end'}
          alignItems={'center'}
          ref={threadContainerThree}
        >
          <Img ref={cogThree} src={cogSvg} size={workSize} marginRight={40} />
        </Rect>
        <Rect
          stroke={'black'}
          lineWidth={10}
          width={width}
          height={height}
          layout
          justifyContent={'end'}
          alignItems={'center'}
          ref={threadContainerFour}
        >
          <Img ref={cogFour} src={cogSvg} size={workSize} marginRight={40} />
        </Rect>
      </Rect>
    </Rect>
  )

  allCogs.forEach((c) => c().scale(0))
  const scaleAndRotate = (r: Reference<Img>) =>
    all(r().scale(1, 0.5), r().rotation(r().rotation() + 360 * 3, 2, linear))
  // yield* all(scaleAndRotate(cogOne), scaleAndRotate(cogTwo), scaleAndRotate(cogThree), scaleAndRotate(cogFour))

  const spawnWork = () => {
    const work = createRef<Img>()
    view.add(createWorkImg(work))
    work().alpha(0)
    work().scale(0)
    work().position(work().position().add([0, 100]))
    return work
  }
  function* animateSpawnWork(work: Reference<Img>, startPos: PossibleVector2, timeS = 0.5) {
    work().position(startPos)
    yield* all(
      work().alpha(1, timeS),
      work().scale(1, timeS),
      work().position(work().position().add([0, -100]), timeS, easeInCubic)
    )
  }

  function* moveWorkToThreadContainer(work: Reference<Img>, container: Reference<Rect>) {
    const startPos = work().absolutePosition()
    const endPos = container().absolutePosition()
    logger.info(`endpos ${endPos.x} - ${endPos.y}`)
    yield* tween(1.2, (v) => {
      // work().absolutePosition(map(startPos, endPos, v))
      // work().absolutePosition(Vector2.arcLerp(startPos, endPos, v))
      work().absolutePosition(Vector2.arcLerp(startPos, endPos, linear(v), false))
    })
  }

  const work = spawnWork()
  yield* animateSpawnWork(work, [-600, 400])
  yield* moveWorkToThreadContainer(work, threadContainerOne)
  yield* scaleAndRotate(cogOne)
  yield* all(
    cogOne().scale(0, 0.5),
    cogOne().alpha(0, 0.5),
    work().position.x(work().position.x() - 1600, 1, easeInCubic),
    work().alpha(0, 1.2, easeInCubic)
  )

  const [wTwo, wThree, wFour, wFive, wSix] = [spawnWork(), spawnWork(), spawnWork(), spawnWork(), spawnWork()]

  yield* all(
    animateSpawnWork(wTwo, [-600, 180 * -1], 0.5),
    animateSpawnWork(wThree, [-600, 180 * 0], 0.6),
    animateSpawnWork(wFour, [-600, 180 * 1], 0.7),
    animateSpawnWork(wFive, [-600, 180 * 2], 0.8),
    animateSpawnWork(wSix, [-600, 180 * 3], 0.9)
  )

  yield* all(
    moveWorkToThreadContainer(wTwo, threadContainerOne)
    // moveWorkToThreadContainer(wThree, threadContainerTwo)
    // moveWorkToThreadContainer(wFour, threadContainerThree)
    // moveWorkToThreadContainer(wFive, threadContainerFour)
  )
})
