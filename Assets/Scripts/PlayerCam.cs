using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerCam : MonoBehaviour
{
   

    // camera rotation
    public float sensX = 3;
    public float sensY = 3;
    public Transform cameraTransform;
    public Transform camHolder;

    public float xRotation;
    public float yRotation;

    // ground movement
    private Rigidbody rb;
    private Vector2 moveInput;
    public float moveSpeed = 3f;
    private bool isMoving = false;

    // jumping
    public float jumpForce = 4f;
    private bool isGrounded = true;
    public LayerMask groundLayer;
    private float playerHeight;
    private float raycastDistance;

    // slope handling

    public float maxSlopeAngle = 45f;
    public float slopeRayExtraLength = 0.5f;

    // sprinting

    private bool isSprinting = false;
    public float sprintSpeed = 8f;     // faster when sprinting
    public float sprintAcceleration = 10f;
    private float currentSpeed;     // Dinamically adjusted speed

    // stamina

    public float maxStamina = 100f;
    public float staminaDepletionRate = 20f;    // per sec
    public float staminaRegenerationRate = 15f; // per sec
    private float currentStamina;

    // stamina bar UI
    public Image staminaFill;
    public float staminaRefillDelay = 2f;
    private float lastSprintTime;
    private Slider staminaBar;
    private Image fillImage;
    private GameObject staminaCanvas;

    // crouching

    public float crouchHeight = 0.9f;     // height when crouching
    public float standHeight = 1.8f;
    public float crouchSpeed = 0.75f;      // movement speed while crouching
    public float crouchSprintSpeed = 4f;
    public float crouchTransitionSpeed = 5f; // How fast crouch/stand transition happens
    private bool isCrouching = false;
    private float targetHeight;
    public Transform CameraHolder;
    // private Vector3 targetCenter;

    // stair handling

    public GameObject stepRayUpper;

    public GameObject stepRayLower;

    public float stepHeight = 0.3f;
    public float stepSmooth = 0.1f;

    // wall detection

    public float wallCheckDistance = 0.5f;
    public float wallSlideGravity = 2f;

    private bool isAgainstWall = false;

    // actions
    private InputAction sprintAction;
    public InputAction lookAction;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction crouchAction;

    void RotateCamera()
    {
        Vector2 mouseDelta = lookAction.ReadValue<Vector2>() * 0.1f;

        // float mouseX = mouseDelta.x * Time.deltaTime * sensX;
        // float mouseY = mouseDelta.y * Time.deltaTime * sensY;

        // yRotation += mouseX;
        // xRotation -= mouseY;
        yRotation += mouseDelta.x * sensX;
        transform.rotation = Quaternion.Euler(0, yRotation, 0);

        xRotation -= mouseDelta.y * sensY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);
        camHolder.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    void MovePlayer()
    {
        //  float targetSpeed = isSprinting ? sprintSpeed : isCrouching ? crouchSpeed : moveSpeed;
        float targetSpeed = moveSpeed;

        if (isCrouching)
            targetSpeed *= 0.4f;

        if (isSprinting)
            targetSpeed *= isCrouching ? 1.4f : 2f;
        // smoothly interpolate to target speed
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, sprintAcceleration * Time.deltaTime);

        // apply movement
        Vector2 input = moveAction.ReadValue<Vector2>();

        //  if (Mathf.Abs(input.y) > 0.1f && input.x == 0)
        // {
        //     if (Physics.Raycast(transform.position, transform.forward, 0.5f, groundLayer)) 
        //     {
        //         input.x = 0.2f * Mathf.Sign(Random.Range(-1f, 1f)); // Tiny random diagonal bias
        //     }
        // }

        //  Debug.Log("Move Input: " + input); // Check if WASD/Gamepad inputs register
        Vector3 cameraForward = Vector3.ProjectOnPlane(camHolder.forward, Vector3.up).normalized;
        Vector3 cameraRight = Vector3.ProjectOnPlane(camHolder.right, Vector3.up).normalized;

        cameraForward.y = 0;    // remove vertical component
        cameraForward.Normalize();

        Vector3 move = cameraRight * input.x + cameraForward * input.y;
        move.Normalize();

        if (isGrounded && Physics.Raycast(
            transform.position, Vector3.down,
            out RaycastHit slopeHit, raycastDistance + slopeRayExtraLength))
        {
            float slopeAngle = Vector3.Angle(slopeHit.normal, Vector3.up);
            if (slopeAngle <= maxSlopeAngle)
                move = Vector3.ProjectOnPlane(move, slopeHit.normal).normalized;
            else
                move = Vector3.zero;    // too steep to walk
        }

        Vector3 horizontalVelocity = move * currentSpeed;
        rb.linearVelocity = new Vector3(horizontalVelocity.x, rb.linearVelocity.y, horizontalVelocity.z);
    }

    bool CheckGrounded()
    {
        CapsuleCollider col = GetComponent<CapsuleCollider>();
        if (col == null) return false;

        Vector3 bottom = transform.TransformPoint(col.center - Vector3.up * (col.height / 2 - col.radius));

        float checkDistance = 0.15f;
        float checkRadius = col.radius * 0.9f;

        if (!Physics.CheckSphere(bottom, checkRadius))
            return false;

        RaycastHit hit;
        if (Physics.SphereCast(bottom + Vector3.up * 0.1f, checkRadius * 0.8f, Vector3.down, out hit, checkDistance + 0.1f))
        {
            if (hit.collider.isTrigger || hit.collider == col)
                return false;

            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            return slopeAngle <= maxSlopeAngle;
        }

        return false;
    }

    void Jump()
    {
        Vector3 jumpOrigin = transform.position;
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit groundHit, raycastDistance))
        {
            jumpOrigin = groundHit.point;   // jump from actual ground contact point
        }
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
    }

    // void stepClimb()
    // {
    //     RaycastHit hitLower;

    //     if (Physics.Raycast(stepRayLower.transform.position, cameraTransform.TransformDirection(Vector3.forward), out hitLower, 0.1f))
    //     {
    //         RaycastHit hitUpper;
    //         if (!Physics.Raycast(stepRayUpper.transform.position, cameraTransform.TransformDirection(Vector3.forward), out hitLower, 0.2f))
    //         {
    //             rb.position -= new Vector3(0f, -stepSmooth, 0f);
    //         }
    //     }
    // }

    void stepClimb()
    {
        float sphereRadius = 0.1f;
        float lowerCastDistance = 0.1f;
        float upperCastDistance = 0.2f;

        if (Physics.SphereCast(stepRayLower.transform.position, sphereRadius, transform.forward, out RaycastHit hitLower, lowerCastDistance))
        {
            if (!Physics.SphereCast(stepRayUpper.transform.position, sphereRadius, transform.forward, out _, upperCastDistance))
            {
                float stepHeight = hitLower.point.y - stepRayLower.transform.position.y;
                if (stepHeight > 0 && stepHeight <= stepHeight)
                {
                    rb.position += new Vector3(0f, stepSmooth * Time.fixedDeltaTime, 0f);
                }
            }
        }
    }

    void HandleWallCollision()
    {
        // Check for walls in front of player
        RaycastHit wallHit;
        isAgainstWall = Physics.Raycast(transform.position, transform.forward, 
                                    out wallHit, wallCheckDistance);
        
        // If against wall while airborne
        if (isAgainstWall && !isGrounded)
        {
            // Apply downward force to prevent floating
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 
                                        Mathf.Max(rb.linearVelocity.y, -wallSlideGravity), 
                                        rb.linearVelocity.z);
            
            // Reduce forward movement against walls
            if (moveInput.y > 0.1f) // If pressing forward
            {
                Vector3 vel = rb.linearVelocity;
                vel.z = vel.z * 0.3f; // Reduce forward velocity
                rb.linearVelocity = vel;
            }
        }
    }

    void UpdateStaminaUI()
    {
        if (staminaBar != null)
            staminaBar.value = currentStamina;

        Image fillImage = staminaBar.fillRect.GetComponent<Image>();

        // completely hide fill when stamina is zero
        fillImage.enabled = currentStamina > 0;

        if (fillImage.enabled)
        {
            staminaBar.value = currentStamina; 
        }
    }

    bool ShouldRefillStamina()
    {
        // should refill only if not currently sprinting, stamina bar isn't already full 
        // and 2+ sec elapsed since sprint
        return !isSprinting && ((Time.time - lastSprintTime >= staminaRefillDelay && currentStamina == 0) ||
                (currentStamina < maxStamina && currentStamina != 0));
    }

    void Start()
    {
        // UI components

        // initialize stamina bar
        staminaBar = GameObject.Find("staminaBar").GetComponent<Slider>();

        if (staminaBar != null && staminaFill == null)
            staminaFill = staminaBar.fillRect.GetComponent<Image>();

        // If not attached to the main camera, take control of it
        if (Camera.main != null && Camera.main.transform != transform)
        {
            transform.SetPositionAndRotation(Camera.main.transform.position, Camera.main.transform.rotation);
            Destroy(Camera.main.gameObject); // Remove old camera
            gameObject.tag = "Main Camera"; // Claim the tag
        }

        if (CameraHolder == null)
        {
            CameraHolder = transform.Find("CameraHolder");
        }

        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            Debug.LogWarning("Rigidbody component was missing - one has been added automatically");
        }

        if (cameraTransform == null)
        {
            // Assuming Main Camera is a child of camHolder
            if (camHolder != null && camHolder.childCount > 0)
            {
                cameraTransform = camHolder.GetChild(0);
                cameraTransform.gameObject.tag = "Main Camera"; // Ensure correct tag
            }
            else
            {
                Debug.LogError("Main Camera not found in hierarchy!");
            }
        }

        rb.freezeRotation = true;

        playerHeight = GetComponent<CapsuleCollider>().height * transform.localScale.y;
        raycastDistance = (playerHeight / 2 + 0.2f);

        // setup action keybinds
        moveAction = new InputAction("Move");
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d")
            .With("Up", "<Gamepad>/leftStick/up")
            .With("Down", "<Gamepad>/leftStick/down")
            .With("Left", "<Gamepad>/leftStick/left")
            .With("Right", "<Gamepad>/leftStick/right");

        lookAction = new InputAction("Look", InputActionType.Value);
        lookAction.AddBinding("<Gamepad>/rightStick");
        lookAction.AddBinding("<Mouse>/delta");

        jumpAction = new InputAction("Jump", InputActionType.Value);
        jumpAction.AddBinding("<Keyboard>/space");
        jumpAction.AddBinding("<Gamepad>/a");

        sprintAction = new InputAction("Sprint", InputActionType.Value);
        sprintAction.AddBinding("<Keyboard>/leftShift");
        sprintAction.AddBinding("<Gamepad>/leftStickPress");

        crouchAction = new InputAction("Crouch", InputActionType.Value);
        crouchAction.AddBinding("<Keyboard>/c");
        crouchAction.AddBinding("<Gamepad>/rightStickPress");


        // init stamina
        currentStamina = maxStamina;

        // init stand/crouch state
        CapsuleCollider col = GetComponent<CapsuleCollider>();
        standHeight = col.height;
        targetHeight = standHeight;

        stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepHeight, stepRayUpper.transform.position.z);

        // stepClimb();

        // active actions
        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
        sprintAction.Enable();
        crouchAction.Enable();

        // SANDA- am nevoie de cursor pentru UI
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down * (raycastDistance + slopeRayExtraLength), Color.blue);
        // Replace your ground check with:
        // RaycastHit hit;
        // isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit,
        //             raycastDistance, groundLayer);

        // Visual debug
        // Debug.DrawRay(transform.position, Vector3.down * raycastDistance,
        //              isGrounded ? Color.green : Color.red);

        RotateCamera();

        //Debug.Log("Ground Layer: " + LayerMask.LayerToName(groundLayer.value));
        isGrounded = CheckGrounded();
        // Debug.Log("isGrounded " + isGrounded);

        MovePlayer();


        if (isGrounded && jumpAction.triggered)
        {
            Jump();
        }

        //ApplyJumpPhysics();

        // crouch

        if (crouchAction.triggered)
        {
            isCrouching = !isCrouching;
        }

        float targetHeight = isCrouching ? 0.3f : 0.8f;
        Vector3 newPos = new Vector3(0, targetHeight, 0);
        //asta afecteaza dimensiunea obiectelor din mana - Sanda
        // fixed - crouch nu ar tb sa mai afecteze dimensiunea obiectelor acum - Alex
        CameraHolder.localPosition = Vector3.Lerp(CameraHolder.localPosition, new Vector3(0, targetHeight, 0), crouchTransitionSpeed * Time.deltaTime);

        // Debug.Log("Jump Pressed: " + jumpAction.triggered);
        // Debug.Log("Crouch Pressed: " + crouchAction.triggered);

        // check if sprinting already
        bool wantsToSprint = sprintAction.IsPressed() && currentStamina > 0;
        isMoving = moveAction.ReadValue<Vector2>().magnitude > 0.1f;

        isSprinting = wantsToSprint && isMoving;

        // magnitude > 0.1f ignores accidental input (slight Stick movement) and only looks for intentional input
        if (isSprinting)
        {
            currentStamina -= staminaDepletionRate * Time.deltaTime;
            currentStamina = Mathf.Max(currentStamina, 0);
//              Debug.Log("Out of stamina!");
            lastSprintTime = Time.time;
        }
        else if (ShouldRefillStamina())
        {
            currentStamina += staminaRegenerationRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
        }

        UpdateStaminaUI();
    }

    void FixedUpdate()
    {
        stepClimb();
        // HandleWallCollision() NU merge inca :/. Incerc sa il repar maine cand o sa imbunatatesc si urcatul pe scari - Alex
        HandleWallCollision();
    }

// void HandleStairs() {
//     RaycastHit hit;
//     Vector3 rayStart = transform.position - Vector3.up * (GetComponent<CapsuleCollider>().height / 2 - 0.1f);
    
//     // Check for a stair or slope in front of the player
//     if (Physics.Raycast(rayStart, transform.forward, out hit, 0.5f, groundLayer)) {
//         float stairHeight = hit.point.y - rayStart.y;
        
//         // If it's a stair (not a wall) and within climbable height
//         if (stairHeight > 0 && stairHeight <= maxStairHeight) {
//             // Smoothly lift the player up the stair
//             Vector3 newPos = transform.position;
//             newPos.y = Mathf.Lerp(transform.position.y, hit.point.y + 1f, stairSmoothSpeed * Time.fixedDeltaTime);
//             rb.MovePosition(newPos);
//         }
//     }
// }
    void OnDestroy()
    {
        moveAction?.Disable();
        lookAction?.Disable();
        jumpAction?.Disable();
        sprintAction?.Disable();
        crouchAction?.Disable();
    }


}