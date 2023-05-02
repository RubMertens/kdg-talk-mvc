import { Circle, Img, Knot, Rect, Spline } from '@motion-canvas/2d/lib/components'
import { makeScene2D } from '@motion-canvas/2d/lib/scenes'
import { all, waitFor } from '@motion-canvas/core/lib/flow'
import { Reference, createRef, viaProxy } from '@motion-canvas/core/lib/utils'

import cogSvg from '../images/gear-svgrepo-com.svg'
import dbSvg from '../images/database-1-svgrepo-com.svg'
import workSvg from '../images/work-alt-svgrepo-com.svg'

import { deepLerp, easeInCirc, linear, map, tween } from '@motion-canvas/core/lib/tweening'

const Work = (ref: Reference<Img>) => {
  const workImg = <Img ref={ref} src={workSvg} scale={0} position={[-600, 100]} />

  return workImg
}

export default makeScene2D(function* (view) {
  const cog = createRef<Img>()

  const cogImg = <Img ref={cog} src={cogSvg} scale={0} position={[750, 250]} />
  const workOneRef = createRef<Img>()
  const workOne = Work(workOneRef)
  const workScale = 0.3

  view.add(
    <>
      <Rect lineWidth={20} stroke={'black'} width={'50%'} height={'80%'} x={view.width() / 4 - 50} />
    </>
  )
  view.add(workOne)
  yield* workOneRef().scale(workScale, 0.5)
  yield* workOneRef().position({ x: 430, y: 0 }, 0.5)

  view.add(cogImg)
  yield* all(cog().scale(workScale, 0.5), cog().rotation(cog().rotation() + 360 * 3, 2, linear))
  const workTwoRef = createRef<Img>()
  const workTwo = Work(workTwoRef)

  const workThreeRef = createRef<Img>()
  const workThree = Work(workThreeRef)

  workThree.position.y(workThree.position.y() + 200)
  const workFourRef = createRef<Img>()
  const workFour = Work(workFourRef)
  workFour.position.y(workFour.position.y() + 400)

  view.add(workTwo)
  view.add(workThree)
  view.add(workFour)

  yield* all(
    cog().scale(0, 0.5),
    workTwoRef().scale(workScale, 0.5),
    workThreeRef().scale(workScale, 0.7),
    workFourRef().scale(workScale, 0.8)
  )
  yield* workOneRef().position({ x: -600, y: -200 }, 0.5)

  yield* all(workOneRef().scale(0, 0.5), workTwoRef().position({ x: 430, y: 0 }, 0.5))
  yield* all(cog().scale(workScale, 0.5), cog().rotation(cog().rotation() + 360 * 3, 2, linear))
  yield* all(workTwoRef().position({ x: -600, y: -200 }, 0.5), cog().scale(0, 0.5))

  yield* all(workTwoRef().scale(0, 0.5), workThreeRef().position({ x: 430, y: 0 }, 0.5))
  yield* all(cog().scale(workScale, 0.5), cog().rotation(cog().rotation() + 360 * 3, 2, linear))
  yield* all(workThreeRef().position({ x: -600, y: -200 }, 0.5), cog().scale(0, 0.5))

  yield* all(workThreeRef().scale(0, 0.5), workFourRef().position({ x: 430, y: 0 }, 0.5))
  yield* all(cog().scale(workScale, 0.5), cog().rotation(cog().rotation() + 360 * 3, 2, linear))
  yield* all(workFourRef().position({ x: -600, y: -200 }, 0.5), cog().scale(0, 0.5))
})
