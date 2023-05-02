import { makeProject } from '@motion-canvas/core'

import example from './scenes/example?scene'
import awaitScene from './scenes/awaitScene?scene'

export default makeProject({
  scenes: [example, awaitScene],
})
