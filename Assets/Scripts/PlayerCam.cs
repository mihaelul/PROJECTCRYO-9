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

    private float xRotation;
    private float yRotation;

    // ground movement
    private Rigidbody rb;
    public float moveSpeed = 3f;
    private Vector2 moveInput;
    private bool isMoving = false;

    // jumping
    public float jumpForce = 4f;
    public float fallMultiplier = 2.5f;
    public float ascendMultiplier = 2f;
    private bool isGrounded = true;
    public LayerMask groundLayer;
    private float groundCheckTimer = 0f;
    private float groundCheckDelay = 0.3f;
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

    // actions
    private InputAction sprintAction;
    private InputAction lookAction;
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
        float radius = GetComponent<CapsuleCollider>().radius * 0.9f; // player size = 1f

        Vector3 rayStart = transform.position + Vector3.up * 0.1f; // start slightly above feet

        Vector3[] rayDirections = new Vector3[]
        {
            Vector3.down,                            // center
            Vector3.down + transform.forward * 0.5f, // front
            Vector3.down - transform.forward * 0.5f, // back
            Vector3.down + transform.right * 0.5f,   // right
            Vector3.down - transform.right * 0.5f    // left
        };

        foreach (Vector3 dir in rayDirections)
        {
            if (Physics.Raycast(rayStart, dir, raycastDistance, groundLayer))
                return true;    // at least one ray hit ground
        }
        return false;

        // foreach (RaycastHit hit in hits)
        // {
        //     float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
        //     if (slopeAngle <= 45f)
        //     {
        //         return true;
        //     }
        // }
        // return false;
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

    void ApplyJumpPhysics()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 2f) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (ascendMultiplier - 1.2f) * Time.fixedDeltaTime;
        }
    }

    // void CreateStaminaBar()
    // {
    //     // create canvas
    //     GameObject canvasGO = new GameObject("StaminaCanvas");
    //     Canvas canvas = canvasGO.AddComponent<Canvas>();
    //     canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    //     staminaCanvas.AddComponent<CanvasScaler>();
    //     staminaCanvas.AddComponent<GraphicRaycaster>();

    //     // create slider
    //     GameObject sliderGO = new GameObject("StaminaBar");
    //     sliderGO.transform.SetParent(canvasGO.transform);
    //     staminaBar = sliderGO.AddComponent<Slider>();
    //     //staminaBar.color = new Color32(2, 215, 255, 255);   // light blue

    //     // slider setup
    //     staminaBar.minValue = 0;
    //     staminaBar.maxValue = maxStamina;
    //     staminaBar.wholeNumbers = false;

    //     // set background
    //     GameObject bgGO = new GameObject("Background");
    //     bgGO.transform.SetParent(sliderGO.transform);
    //     Image bgImage = bgGO.AddComponent<Image>();
    //     bgImage.color = new Color32(155, 184, 195, 255);   // blue-ish grey

    //     // fill area
    //     GameObject fillAreaGO = new GameObject("Fill Area");
    //     fillAreaGO.transform.SetParent(sliderGO.transform);
    //     RectTransform fillAreaRect = fillAreaGO.AddComponent<RectTransform>();
    //     fillAreaRect.sizeDelta = new Vector2(-20, 0); // Padding

    //     // fill
    //     GameObject fillGO = new GameObject("Fill");
    //     fillGO.transform.SetParent(fillAreaGO.transform);
    //     fillImage.color = new Color32(2, 215, 255, 255);   // light blue
    //     fillImage.type = Image.Type.Filled;
    //     fillImage.fillMethod = Image.FillMethod.Horizontal;

    //     staminaBar.handleRect = null;
    // }

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
        // targetCenter = col.center;

        // active actions
        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
        sprintAction.Enable();
        crouchAction.Enable();

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        // SANDA- am nevoie de cursor pentru UI
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down * (raycastDistance + slopeRayExtraLength), Color.blue);
        // Replace your ground check with:
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit,
                    raycastDistance, groundLayer);

        // Visual debug
        // Debug.DrawRay(transform.position, Vector3.down * raycastDistance,
        //              isGrounded ? Color.green : Color.red);

        RotateCamera();

        isGrounded = CheckGrounded();

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

        Debug.Log("Jump Pressed: " + jumpAction.triggered);
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
    void OnDestroy()
    {
        moveAction?.Disable();
        lookAction?.Disable();
        jumpAction?.Disable();
        sprintAction?.Disable();
        crouchAction?.Disable();
    }


}